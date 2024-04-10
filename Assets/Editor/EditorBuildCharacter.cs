using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System;
using System.IO;


/// To do: 
/// at some point we will delete all other character animations clips and animation controllers. 
/// And ony have one: baseCharacter prefab. 
/// then we'll create prefabs from the baseCharacter prefab. 

/// <summary>
/// We'll use this in the editor to instantiate charaters 
/// that all other characters will derive from by swapping sprites
/// 
/// </summary>


// Steps, roughly: 
// build the prefab for the base character
// build gameobject
// build idle, walk, attack frame objects
// - set these to "unchecked": activeSelf = false
// build animation component
// build all other pieces associated with the game object

// - instantiate the animation clips
// instantiate
// build the animation clips based on the child objects
// build a function takes a series of sprites maybe other inputs like stats and builds the character 
// for starters, lets create idleDown 


public class EditorBuildCharacter : MonoBehaviour
{
    /// <summary>
    /// Build the base character that all other characters will derive from 
    /// </summary>
    /// <param name="charName"></param>
    /// <returns></returns>
    public static GameObject BuildCharacter(string charName){
        GameObject character = GameObject2D("baseCharacter");
        CreateStates(character, charName, "idle", "walk", "0", "");
        string[] walkSuffixes = {"0", "1", "3"};
        foreach (string walkSuffix in walkSuffixes ){
            CreateStates(character, charName, "walk", "walk", walkSuffix, walkSuffix);
        }
        return character;         
    }

    /// <summary>
    /// Replication of creation of a gameobject2d in script.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static GameObject GameObject2D(string name){
        GameObject box = new GameObject(name);
        var spriteRenderer = box.AddComponent<SpriteRenderer>();
        box.transform.position = new Vector3(0f, 0f, 0f);
        return box; 
    }

    /// <summary>
    /// Create a 2d game object with specific settings that we'll use for
    /// sprite child-like objects (e.g. idleWalk, walkDown0). 
    /// </summary>
    /// <param name="objectName">name of the new object (e.g. idleWalk) </param>
    /// <param name="spritePath">path of the sprite relative to resources (e.g. /locke/locke_walk_left2)</param>
    /// <returns></returns>
    public static GameObject CreateDisabledSpritesForStates(string objectName, string spritePath){
        GameObject newGameObject = GameObject2D(objectName);
        Sprite newSprite = Resources.Load<Sprite>(spritePath);
        newGameObject.SetActive(false); 
        newGameObject.GetComponent<SpriteRenderer>().sprite = newSprite; 
        return newGameObject; 
    }


    /// <summary>
    /// Given a character, instantiate all of the states for a given state
    /// </summary>
    /// <param name="character">parent character gameObject </param>
    /// <param name="charName">name of character for file path purposes</param>
    /// <param name="actionState">gameObject action (e.g. idle, walk) as defined in the gameObject </param>
    /// <param name="spriteActionState">sprite action (e.g. walk) as defined in the sprite </param>
    /// <param name="stateFileSuffix">sprite file suffix (e.g. 0, 1, 2) </param>
    /// <param name="actionStateSuffix">game object suffix (e.g. 0, 1, 2; idle has none)</param>
    public static void CreateStates(GameObject character, string charName, string actionState, string spriteActionState, string stateFileSuffix, string actionStateSuffix){
        string baseFilePath = "Sprites/" + charName + "/"; 

        string[] directions = {"down", "up", "right", "left"};
        string[] spriteDirections = {"front", "back", "right", "left"};
        List<string> stateNames = new();
        List<string> stateSpriteFiles = new();
        for (int i = 0; i < directions.Length; i ++) {
            string direction = directions[i];
            string spriteDirection = spriteDirections[i]; 
            string stateName = actionState + char.ToUpper(direction[0]) + direction.Substring(1) + actionStateSuffix; 
            string stateSpriteFileName = charName + "_" + spriteActionState + "_" + spriteDirection +stateFileSuffix ; 
            stateNames.Add(stateName); 
            stateSpriteFiles.Add(stateSpriteFileName);
            
            GameObject stateGameObject = CreateDisabledSpritesForStates(stateName, baseFilePath + stateSpriteFileName);
            stateGameObject.GetComponent<SpriteRenderer>().sortingOrder = 3; 
            stateGameObject.transform.parent = character.transform;
        }
    }

    /// <summary>
    /// Given the char name, instantiate the character as a game object and create a prefab (disable later? ). 
    /// This is based on baseCharacter, where all other characters will be based on. 
    /// </summary>
    /// <param name="characterName">character's name</param>
    /// <returns></returns>
    public static GameObject InstantiateCharacterFromBaseCharacter(string characterName){
        // instantiate prefab
        GameObject charPrefab = Resources.Load("Prefabs/Characters/baseCharacter", typeof(GameObject)) as GameObject;            
        // create game object
        GameObject charInstance = Instantiate(charPrefab, new Vector3( 0.0f, 0.0f, 0.0f), Quaternion.identity);
        charInstance.name = characterName; 

        // change the sprites
        string[] directions = {"down", "up", "right", "left"};
        string[] spriteDirections = {"front", "back", "right", "left"};
        string[] spriteSuffixes = {"0", "1", "3"}; 

        // idle         
        for (int i = 0; i < directions.Length; i++ ){
            string direction = directions[i];
            string spriteDirection = spriteDirections[i];
            ChangeSprite(charInstance,  characterName, "idle", "walk", direction, spriteDirection, "0", "" );

        }
        // walk0, 1, 3
        foreach (string spriteSuffix in spriteSuffixes){
            for (int i = 0; i < directions.Length; i++ ){
                string direction = directions[i];
                string spriteDirection = spriteDirections[i];
                ChangeSprite(charInstance,  characterName, "walk", "walk", direction, spriteDirection, spriteSuffix, spriteSuffix );
            }
        }
        // save the prefab

        return charPrefab;
    }

    /// <summary>
    /// Given the parent and some parameters that will define the path of the sprites, swap out the sprites from the parent to the child. 
    /// Use this to change up the baseCharacter with different sprites to exchange the animation logic
    /// </summary>
    /// <param name="parent"> The parent object reference, most likely, the base character. </param>
    /// <param name="charName">The char name. </param>
    /// <param name="actionString">"action" of the animation, as defined by the gameObject name. </param>
    /// <param name="stateFileBaseName">action of the sprite animation, as defined by the sprite. </param>
    /// <param name="direction">up, down, left, or right</param>
    /// <param name="spriteDirection">left, right, front, back, as defined by the sprite</param>
    /// <param name="spriteDirectionSuffix">variants of the sprite (e.g. 0, 1, 3) </param>
    /// <param name="objectSuffix">variants of the game object (e.g. 0, 1, 3)</param>
    public static void ChangeSprite(GameObject parent, string charName, 
            string actionString, string stateFileBaseName, 
            string direction, string spriteDirection, 
            string spriteDirectionSuffix, string objectSuffix){

        string childName = actionString + char.ToUpper(direction[0]) + direction.Substring(1) + objectSuffix;
        Transform childTransform = parent.transform.Find(childName); 
        Debug.Log(childTransform.name);
        // define the path and file name
        string baseFilePath = "Sprites/" + charName + "/"; 
        string stateSpriteFileName = charName + "_" + stateFileBaseName + "_" + spriteDirection + spriteDirectionSuffix ; 
        // change the sprite
        Sprite newSprite = Resources.Load<Sprite>(baseFilePath + stateSpriteFileName);
        childTransform.GetComponent<SpriteRenderer>().sprite = newSprite; 

    }


}

using System;
using System.Collections.Generic;
using UnityEngine;
/*
CharactersObject handles instantiation of the characters in the beginning of the 
round render. 
*/
public class CharactersObject : MonoBehaviour 
{

    public static void Initialize(GameManager instance)
    {
        InstantiateChars(instance);
    }

    // this takes in the prefabs/characters path!
    public static GameObject InstantiateChar(GameManager Instance, string pfFileName, Coordinate w)
    {
        Board board = Instance.board;  
        (Double u, Double v) = board.ConvertMatToSceneCoords(w);
        if (u == -1 & v == -1)
        {
            throw new KeyNotFoundException(" bad coordinate " + w + " given.");
        }

        // Vector2 vector = Board.ConvertCoordinateToVector2(w);

        GameObject charPrefab = Resources.Load("Prefabs/Characters/" + pfFileName, typeof(GameObject)) as GameObject;            
        GameObject charInstance = Instantiate(charPrefab, new Vector3((float)u, (float)v, 0.0f), Quaternion.identity);

        GameObject healthBarPrefab = Resources.Load("Prefabs/CharacterUI/HealthBar", typeof(GameObject)) as GameObject;        
        GameObject healthBarInstance = Instantiate(healthBarPrefab, charInstance.transform);

        return charInstance; 
        


    }
    // manage Components to Char here. 
    public static void AddComponentsToChar(GameManager Instance, int id, string charName, int hp, int attack, int moves, int atkRange, Boolean isYourTeam)
    {
        Board board = Instance.board; 
        GameObject[] charArray = Instance.charArray; 

        GameObject charInstance = charArray[id];        
        // Add components (scripts) to the char instance
        charInstance.AddComponent<CharacterStats>();
        charInstance.GetComponent<CharacterStats>().Initialize(charName, hp, attack, moves, atkRange); 
        charInstance.AddComponent<CharacterMove>();
        charInstance.GetComponent<CharacterMove>().Initialize(charInstance, id, Instance);
        charInstance.AddComponent<CharacterGameState>(); 
        charInstance.GetComponent<CharacterGameState>().Initialize(Instance, id, isYourTeam); 
        charInstance.AddComponent<CharacterBattle>(); 
        charInstance.AddComponent<Rigidbody2D>();
        charInstance.GetComponent<Rigidbody2D>().gravityScale = 0;      
        charInstance.AddComponent<CharacterAnimateController>();
        charInstance.GetComponent<CharacterAnimateController>().Initialize(Instance, id, charInstance); 
    }

    // Spawn characters from a set of predefined chars; 
    // will update later on how the level will define who you put on the map
    // should be called at the beginning and only once. 
    // Id's start at 0. 
    public static GameObject[] InstantiateChars(GameManager Instance)
    {
        Board board = Instance.board;
        GameObject[] charArray = Instance.charArray; 

        // temporarily we'll provide the chars we want to instantiate for testing; 
        // later we'll build some mechanism to do this; perhaps move this class outside the game manager. 
        (String, int, int, int, int, int, int, Boolean)[] chars = {
            ("locke",       9, 5, 7, 0, 0, 1, true),            // was "glenn"
            ("celes",       6, 2, 3, 1, 0, 1, true), 
            ("gerad",       12, 3, 3, 0, 1, 1, false), 
            ("sabin",       5, 6, 3, 1, 1, 1, false)
            };
        
        // GameObject[] charArray = new GameObject[chars.Length]; 
        // NOTE! the id is simply the array entry of the character in charArray; 
        for (int id = 0; id < chars.Length; id ++)
        {
            // first, instantiate the char
            (String charName, int hp, int atk, int moves, int x, int y, int atkRange, Boolean isYourTeam) = chars[id];
            Coordinate w = new Coordinate(x, y);
            GameObject curChar = InstantiateChar(Instance, charName, w);            
            board.Put(w, id);
            charArray[id] = curChar;

            // then, add components; anything requiring id needs to be called after the board/charArray is built
            AddComponentsToChar(Instance, id, charName, hp, atk, moves, atkRange, isYourTeam);
        }
        return charArray; 

    }

}
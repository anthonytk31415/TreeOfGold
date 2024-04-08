using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.TextCore.Text;


public class ButtonTest1 : MonoBehaviour
{
    // GameManager instance; 
    // GameObject character;
    // Rigidbody2D rb;
    // Animator animator;
    [SerializeField] Button testButton; 

    public void Start ()
    {
    }

    public void Update ()
    {
        // UpdateTest();
    }
    // public void Instantiate(){
    //     rb = character.GetComponent<Rigidbody2D>();
    //     animator = character.GetComponent<Animator>();
    // }


    public void OnButtonClick(){
        // Vector2 originalPosition = rb.position; 
        // Vector2 delta = new Vector2(-1, 0);
        // Instantiate();
        GameManager instance = GameObject.FindObjectOfType<GameManager>(); 
        GameObject character = instance.charArray[0];
        character.GetComponent<CharacterAnimateController>().InstantiateTraversal(Direction.left);
    }

    // public bool traversal; 
    // public Vector2 destination; 

    // // define this on left only 
    // public void InstantiateTraversal(){
    //     this.traversal = true;
    //     Vector2 startPos = character.transform.position;
    //     this.destination = startPos + Vector2.left;
    //     animator.SetBool("walkLeftBool", true);

    // }

    // public void UpdateTest() {
    //     if (this.traversal){            
    //         character.transform.Translate(Vector2.left * Time.deltaTime); 
    //         if (Vector2.Distance(character.transform.position, this.destination) < .001f){            
    //             character.transform.position = this.destination; 
    //             this.traversal = false; 
    //         }
    //     }


    //     // Vector2 startPos = character.transform.position; 
    //     // Vector2 endPos = startPos + 3*Vector2.right;
    //     // Debug.Log(endPos + "; " + startPos);
    //     // Vector2 curPos = startPos; 
    //     // // float factor = 30f;
    //     // Vector2 increment = Vector2.right * Time.deltaTime/1000000;
    //     // Debug.Log(Time.deltaTime);

    //     // while (Vector2.Distance(curPos, endPos)){;
    //     //     character.transform.position = curPos + increment;
    //     //     curPos = character.transform.position; 
    //     // }

    //     // put this iin the update of the gameobject. 
    //     // if a flag is true: do the increment method
    //     // at the end of the method, if the position meets a criteria, set the gameobject element to off




    // }




    // public IEnumerator Test2() {
    //     int charId = 0; 
    //     Coordinate v = new Coordinate(3, 0);
    //     yield return instance.characterAnimateController.GetComponent<CharacterAnimateController>()
    //             .AnimateMoveChar(charId, v); 
    // }


    // // given a starting point and pathlist, return a list of directions to get from 
    // // start to finish




    // public void idleBoolFalse(){
    //     // Debug.Log(animator);
    //     // animator.SetBool("idleDownBool", true);
    //     // string[] idles = new string[] {"idleDownBool", "idleUpBool", "walkDownBool"};
    //     foreach (var param in animator.parameters){
    //         animator.SetBool(param.name, false);
    //     }
    //     // foreach ( int animator.parameter idle in idles){
    //     //     animator.SetBool(idle, false);
    //     // }
    //     animator.SetBool("idleDownBool", true);

    // }

}

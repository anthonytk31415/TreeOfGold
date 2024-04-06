using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;

public class ButtonTest : MonoBehaviour
{
    [SerializeField] Button testButton; 
    public void OnButtonClick(){
        GameManager instance = GameObject.FindObjectOfType<GameManager>();
        //GameObject lena = instance.charArray[0];
        //Rigidbody2D rb = lena.GetComponent<Rigidbody2D>();
        //Animator animator = lena.GetComponent<Animator>();

        //Vector2 originalPosition = rb.position; 
        //Vector2 delta = new Vector2(-1, 0);

        //// foreach (var parameter in animator.parameters){
        ////     Debug.Log(parameter.name);
        //// }

        //Debug.Log("Touched idleUpTrigger...");
        //animator.SetBool("idleUpBool", true);


        //rb.MovePosition(originalPosition + delta); 
        List<Direction> myList = new List<Direction> { Direction.down, Direction.up };
        StartCoroutine(instance.characterAnimateController.GetComponent<CharacterAnimateController>().ApplyMoves(0, myList));
    }
        

}

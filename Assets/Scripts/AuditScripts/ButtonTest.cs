using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;
using UnityEditor.Animations;

public class ButtonTest : MonoBehaviour
{
    [SerializeField] Button testButton; 
    public void OnButtonClick(){
        GameManager instance = GameObject.FindObjectOfType<GameManager>();
        GameObject character = instance.charArray[1];
        // character.GetComponent<Animator>().SetTrigger("idleBlinkBlackDownTrigger");
        character.GetComponent<CharacterAnimateController>().characterAnimateCommandBlinkBlack.DoMove(Direction.down);


    }
        

}

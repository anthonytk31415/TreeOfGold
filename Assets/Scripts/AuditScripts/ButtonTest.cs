using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;

public class ButtonTest : MonoBehaviour
{
    [SerializeField] Button testButton; 
    public void OnButtonClick(){
        GameManager instance = GameObject.FindObjectOfType<GameManager>();
        GameObject character = instance.charArray[1];
        character.GetComponent<CharacterAnimateController>().AnimateAttackSword(Direction.down);
        Debug.Log("attacking: " + Direction.down);
    }
        

}

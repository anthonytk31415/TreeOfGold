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
    }

    public void OnButtonClick(){
        GameManager instance = GameObject.FindObjectOfType<GameManager>();
        GameObject character = instance.charArray[1];
        // character.GetComponent<Animation>().Play("default");
        // character.GetComponent<Animation>().Play("idleBlinkBlackDown");
    }

}

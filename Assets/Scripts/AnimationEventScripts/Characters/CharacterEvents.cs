using UnityEngine;
using System.Collections;


public class CharacterEvents : MonoBehaviour
{
    public void WalkToIdleDown()
    {
        // SetFalseParameters();
        // GetComponent<Animator>().SetBool("idleDownBool", true);
    }

    public void WalkToIdleUp()
    {
        // SetFalseParameters();
        // GetComponent<Animator>().SetBool("walkUpBool", false);
    }

    public void WalkToIdleLeft()
    {
        // SetFalseParameters();
        // GetComponent<Animator>().SetBool("walkLeftBool", false);
    }

    public void WalkToIdleRight()
    {
        // SetFalseParameters();
        // GetComponent<Animator>().SetBool("walkRightBool", false);
    }

    public void SetFalseParameters(){
        Animator animator= GetComponent<Animator>();
        foreach (var param in animator.parameters)
        {
            animator.SetBool(param.name, false);
        }
    }
}
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections;

public class ButtonTest1 : MonoBehaviour
{
    GameManager instance; 
    GameObject lena;
    Rigidbody2D rb;
    Animator animator;
    [SerializeField] Button testButton; 

    public void Start (){
    }

    public void Instantiate(){
        instance = GameObject.FindObjectOfType<GameManager>(); 
        lena = instance.charArray[0];
        rb = lena.GetComponent<Rigidbody2D>();
        animator = lena.GetComponent<Animator>();
    }


    public void OnButtonClick(){
        // Vector2 originalPosition = rb.position; 
        // Vector2 delta = new Vector2(-1, 0);
        Instantiate();
        Debug.Log("Resetting idleDownTrigger...");

        StartCoroutine(Test());

    }

    public IEnumerator Test() {

        //idleBoolFalse();
        yield return instance.characterAnimateController.GetComponent<CharacterAnimateController>().ApplyTranslationDir(0, Direction.up);
        // animator.SetBool("idleDownBool", true);
        // yield return new WaitForSeconds(1.0f);
        // idleBoolFalse();
        // animator.SetBool("idleUpBool", true);
        // yield return new WaitForSeconds(1.0f);
        // idleBoolFalse();
        // animator.SetBool("idleDownBool", true);
        // yield return new WaitForSeconds(1.0f);
        // idleBoolFalse();        
        // animator.SetBool("walkDownBool", true);
        // yield return new WaitForSeconds(1.0f);

    }



    public void idleBoolFalse(){
        // Debug.Log(animator);
        // animator.SetBool("idleDownBool", true);
        // string[] idles = new string[] {"idleDownBool", "idleUpBool", "walkDownBool"};
        foreach (var param in animator.parameters){
            animator.SetBool(param.name, false);
        }
        // foreach ( int animator.parameter idle in idles){
        //     animator.SetBool(idle, false);
        // }
        animator.SetBool("idleDownBool", true);

    }

}

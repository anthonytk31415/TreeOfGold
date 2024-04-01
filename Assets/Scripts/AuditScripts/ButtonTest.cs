using UnityEngine;
using UnityEngine.UIElements;
using System;

public class ButtonTest : MonoBehaviour
{
    [SerializeField] Button testButton; 
    public void OnButtonClick(){
        Debug.Log("testing hello world");
        GameManager instance = GameObject.FindObjectOfType<GameManager>(); 
        Debug.Log(instance);
        GameObject lena = instance.charArray[0];
        Debug.Log(lena);
        Rigidbody2D rb = lena.GetComponent<Rigidbody2D>();
        Debug.Log(rb);

        Vector2 originalPosition = rb.position; 
        Vector2 delta = new Vector2(-1, 0);

        StartCoroutine(CoroutineUtils.Lerp(.5f, 8,  
            t => rb.MovePosition(originalPosition + t*delta))
        );
    }

}

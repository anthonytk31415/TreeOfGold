using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// whats the flow here? 
// you have 2 chars, player and enemy, and you execute the move. might as well create this instance on the 


// uses StartCoroutine to initiate animations

public class BattleManager: MonoBehaviour 
{
    private GameManager instance; 
    private Board board;

    public void Initialize(GameManager instance){
        // GameObject battleManager = new GameObject("Battle Manager");
        this.instance = instance; 
        this.board = instance.board; 

    }

    public IEnumerator TriggerBlackThenRed(GameObject obj1, GameObject obj2){
        yield return StartCoroutine(TriggerBlinkBlack(obj1));
        yield return StartCoroutine(TriggerBlinkRed(obj2));
    }

    IEnumerator TriggerBlinkBlack(GameObject obj)
    {  
        SpriteRenderer spR = obj.GetComponent<SpriteRenderer>(); 
        BlinkBlack(spR);
        yield return new WaitForSeconds(0.1f); 
        BlinkWhite(spR);
        yield return new WaitForSeconds(0.1f); 
        BlinkBlack(spR);
        yield return new WaitForSeconds(0.1f); 
        BlinkWhite(spR);
        yield return new WaitForSeconds(0.15f);
    }


    IEnumerator TriggerBlinkRed(GameObject obj)
    {  
        SpriteRenderer spR = obj.GetComponent<SpriteRenderer>(); 
        BlinkRed(spR);
        yield return new WaitForSeconds(0.15f); 
        // BlinkWhite(spR);
        // yield return new WaitForSeconds(0.05f); 
        // BlinkRed(spR);
        // yield return new WaitForSeconds(0.05f); 
        BlinkWhite(spR);
        yield return new WaitForSeconds(0.25f);

    }

    private void BlinkRed(SpriteRenderer spriteRenderer){
        spriteRenderer.color = Color.red; 
    }
    private void BlinkWhite(SpriteRenderer spriteRenderer){
        spriteRenderer.color = Color.white; 
    }
    private void BlinkBlack(SpriteRenderer spriteRenderer){
        spriteRenderer.color = Color.black; 
    }


    public IEnumerator DeathFadeout(GameObject obj)
    {
        float fadeDuration = 0.75f;
        Renderer rend = obj.GetComponent<Renderer>();
        Color originalColor = rend.material.color;        
        float alpha = 1.0f;
        rend.material.color = Color.red; 
        // Loop until the alpha value is 0
        while (alpha > 0.0f)
        {
            alpha -= Time.deltaTime / fadeDuration;
            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            rend.material.color = newColor;

            // Wait for the next frame
            yield return null;
        }


    }





}
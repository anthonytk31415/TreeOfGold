using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPhaseScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static IEnumerator InstantiatePlayerPhaseObject(){

        // Lock mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameObject playerPhase = Resources.Load("Prefabs/PhaseBanner/PlayerPhaseCanvas", typeof(GameObject)) as GameObject;        
        GameObject playerPhaseInstance = Instantiate(playerPhase);
        Transform playerPhaseTextTransform = playerPhaseInstance.transform.Find("BackgroundPanel/PlayerPhaseText");
        if (playerPhaseTextTransform != null){
            Debug.Log(playerPhaseTextTransform.GetComponent<TextMeshProUGUI>().color);
            TextMeshProUGUI textObj = playerPhaseTextTransform.GetComponent<TextMeshProUGUI>(); 
            yield return FadeToAlpha(0.5f, textObj);
        }
        Destroy(playerPhaseInstance);

        // Unlock mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;


        yield return new WaitForSeconds(0.5f);
    
    }

    public static IEnumerator FadeToAlpha(float fadeDuration, TextMeshProUGUI textObj){
        float ticks = 10f; 
        float tickUnit = fadeDuration/ticks; 
        float curTick = 0f; 
        while (curTick < fadeDuration)
        {
            float percentageComplete = curTick/fadeDuration; 
            float pctAlpha = 1-percentageComplete; 
            textObj.alpha = pctAlpha;
            curTick += tickUnit;
            yield return new WaitForSeconds(tickUnit);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class PhaseBannerManager : MonoBehaviour
{

    public PhaseBannerFactory phaseBannerFactory; 

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static IEnumerator InstantiateBanner(GameManager instance, PhaseBanner phaseBanner){
        PhaseBannerFactory phaseBannerFactory = instance.phaseBannerFactory;
        // Lock mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameObject playerPhase = Resources.Load(phaseBannerFactory.getCanvas(phaseBanner), typeof(GameObject)) as GameObject;        
        Debug.Log(playerPhase);
        GameObject playerPhaseInstance = Instantiate(playerPhase);
        Transform playerPhaseTextTransform = playerPhaseInstance.transform.Find(phaseBannerFactory.getText(phaseBanner));
        if (playerPhaseTextTransform != null){
            TextMeshProUGUI textObj = playerPhaseTextTransform.GetComponent<TextMeshProUGUI>(); 
            textObj.alpha = 0f;
            yield return UnFadeToAlpha(0.5f, textObj);
        }
        Destroy(playerPhaseInstance);
        // Unlock mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        yield return new WaitForSeconds(0.5f);    
    }

    public static IEnumerator UnFadeToAlpha(float fadeDuration, TextMeshProUGUI textObj){
        float ticks = 10f; 
        float tickUnit = fadeDuration/ticks; 
        float curTick = 0f; 
        while (curTick < fadeDuration)
        {
            float percentageComplete = curTick/fadeDuration; 
            float pctAlpha = percentageComplete; 
            textObj.alpha = pctAlpha;
            curTick += tickUnit;
            yield return new WaitForSeconds(tickUnit);
        }
    }
}

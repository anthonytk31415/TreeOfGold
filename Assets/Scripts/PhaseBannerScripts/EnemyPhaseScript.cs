using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPhaseScript : MonoBehaviour
{

    // public delegate void CoroutineFinished();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static IEnumerator DisplayEnemyPhaseBanner(){
        Debug.Log("enemy phase called. ");
        // Lock mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameObject enemyPhase = Resources.Load("Prefabs/PhaseBanner/EnemyPhaseCanvas", typeof(GameObject)) as GameObject;        
        GameObject enemyPhaseInstance = Instantiate(enemyPhase);
        Transform enemyPhaseTextTransform = enemyPhaseInstance.transform.Find("BackgroundPanel/EnemyPhaseText");
        if (enemyPhaseTextTransform != null){
            TextMeshProUGUI textObj = enemyPhaseTextTransform.GetComponent<TextMeshProUGUI>(); 
            yield return FadeToAlpha(0.5f, textObj);
        }
        Destroy(enemyPhaseInstance);

        // Unlock mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // call next set of functions after this is done.
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

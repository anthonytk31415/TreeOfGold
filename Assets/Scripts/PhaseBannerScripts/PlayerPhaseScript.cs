using System.Collections;
using System.Collections.Generic;
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
        GameObject playerPhase = Resources.Load("Prefabs/PhaseBanner/PlayerPhaseCanvas", typeof(GameObject)) as GameObject;        
        GameObject playerPhaseInstance = Instantiate(playerPhase);
        Transform playerPhaseTextTransform = playerPhaseInstance.transform.Find("PlayerPhaseText");
        Debug.Log(playerPhaseTextTransform);
        if (playerPhaseTextTransform != null){
            Debug.Log(playerPhaseTextTransform.GetComponent<Text>());
        }
        yield return new WaitForSeconds(0.5f);

        Destroy(playerPhaseInstance);
        yield return new WaitForSeconds(0.5f);

    
    }
}

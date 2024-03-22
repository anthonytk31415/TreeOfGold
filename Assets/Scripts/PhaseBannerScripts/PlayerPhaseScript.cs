using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public static void InstantiatePlayerPhaseObject(){
        GameObject playerPhase = Resources.Load("Prefabs/PhaseBanner/PlayerPhaseCanvas", typeof(GameObject)) as GameObject;        
        GameObject playerPhaseInstance = Instantiate(playerPhase);
    
    }
}

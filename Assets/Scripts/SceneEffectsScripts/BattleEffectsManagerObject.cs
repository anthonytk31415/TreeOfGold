using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This controls on-screen effects when battles occur. 
/// </summary>
public class BattleEffectsManagerObject : MonoBehaviour {
    public static GameObject Initialize(GameManager instance) {
        GameObject battleEffectsManagerObject = new GameObject("BattleEffectsManagerObject");
        battleEffectsManagerObject.AddComponent<BattleEffectsManager>();
        battleEffectsManagerObject.GetComponent<BattleEffectsManager>().Initialize(instance);
        return battleEffectsManagerObject; 
    }    
    
}

//
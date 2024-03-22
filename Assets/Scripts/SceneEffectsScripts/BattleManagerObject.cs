using System;
using System.Collections.Generic;
using UnityEngine;


// Object for MoveController Instantiation. 

public class BattleManagerObject : MonoBehaviour {
    public static GameObject Initialize(GameManager instance) {
        GameObject battleManagerObject = new GameObject("BattleManagerObject");
        battleManagerObject.AddComponent<BattleManager>();
        battleManagerObject.GetComponent<BattleManager>().Initialize(instance);
        return battleManagerObject; 
    }    
    
}
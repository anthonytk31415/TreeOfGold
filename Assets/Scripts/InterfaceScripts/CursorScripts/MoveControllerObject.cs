using System;
using System.Collections.Generic;
using UnityEngine;

// Object for MoveController Instantiation. 


public class MoveControllerObject : MonoBehaviour {

    public static GameObject Initialize(GameManager instance) {
        GameObject moveControllerObj = new GameObject("MoveControllerObject");
        moveControllerObj.AddComponent<MoveController>();
        moveControllerObj.GetComponent<MoveController>().Initialize(instance);
        return moveControllerObj; 
    }    
}
using System;
using System.Collections.Generic;
using UnityEngine;


// Object for MoveController Instantiation. 

public class MoveControllerObject : MonoBehaviour {

    public static GameObject Initialize(Board board, GameManager instance) {
        // GameObject curPrefab = Resources.Load("Prefabs/InterfaceElements/" + "cursor", typeof(GameObject)) as GameObject;
        GameObject moveControllerObj = new GameObject("MoveControllerObject");
        // GameObject cursorInstance = Instantiate(curPrefab, new Vector3(2f, 5f, 0f), Quaternion.identity);
        moveControllerObj.AddComponent<MoveController>();
        moveControllerObj.GetComponent<MoveController>().Initialize(board, instance);
        return moveControllerObj; 
    }    
    
}
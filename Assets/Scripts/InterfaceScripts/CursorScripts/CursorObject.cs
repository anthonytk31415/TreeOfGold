using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;

// heres where I implement the cursor controller instantiation 
// still need to think what i do here. 


public class CursorObject : MonoBehaviour {

    public static GameObject Initialize(Board board, GameManager instance) {
        GameObject curPrefab = Resources.Load("Prefabs/InterfaceElements/" + "cursor", typeof(GameObject)) as GameObject;
        GameObject cursorInstance = Instantiate(curPrefab, new Vector3(2f, 5f, 0f), Quaternion.identity);
        cursorInstance.AddComponent<ArrowMove>();
        cursorInstance.GetComponent<ArrowMove>().Initialize(board, instance);
        return cursorInstance; 
    }    
    
}
// using System.Collections; 
// using System.Collections.Generic; 
// using UnityEngine;

// // Camera

// public class CameraManager : MonoBehaviour {

//     public static Transform Initialize(int width, int height){

//         Transform cameraPrefab = Resources.Load("Prefabs/Camera/" + "MainCamera", typeof(Transform)) as Transform;            
//         Transform camera = Instantiate(cameraPrefab, new Vector3((float)width/2 -0.5f, (float)height / 2 - 0.5f,-10), Quaternion.identity);

//         // _cam.transform.position = new Vector3((float)_width/2 -0.5f, (float)_height / 2 - 0.5f,-10);
//         return camera; 
//     }
// }
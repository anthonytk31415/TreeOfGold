// // We will use this CharSpawner in the GameManager to 
// // instantiate our army characters

// using System;
// using System.Collections.Generic;
// using UnityEngine;


// // public class CharSpawner : MonoBehaviour
// // {
// //     public void SpawnChar(String pfFileName, int hp, int attack, Double x, Double y)
// //     {
// //         GameObject charPrefab = Resources.Load("Prefabs/Characters/" + pfFileName, typeof(GameObject)) as GameObject;
// //         GameObject charInstance = Instantiate(charPrefab, new Vector3((float)x, (float)y, 0.0f), Quaternion.identity);

// //         // Add components (scripts) to the char instance
// //         charInstance.AddComponent<CharacterStats>();
// //         CharacterStats charStats = charInstance.GetComponent<CharacterStats>();

// //         // assign stats to char
// //         charStats.hp = hp;
// //         charStats.attack = attack;
// //     }

// // }
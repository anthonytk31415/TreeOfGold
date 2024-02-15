using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

using TMPro; 
public class StatMenuManager : MonoBehaviour
{

    [SerializeField] GameManager Instance; 
    [SerializeField] Board board; 
    // [SerializeField] TextMeshProUGUI textName; 
    // [SerializeField] TextMeshProUGUI textHp; 
    // [SerializeField] TextMeshProUGUI textAttack; 
    // call this when a unit is selected 
    [SerializeField] TextMeshProUGUI[] playerUnitTextComponents; 
    [SerializeField] TextMeshProUGUI[] enemyUnitTextComponents; 


    void Start() {
        MoveController.OnSelectedCharIdChanged += HandleCharIdChanged;
        MoveController.OnSelectedEnemyIdChanged += HandleEnemyIdChanged;
        BlankPlayerTextBoxes();
        BlankEnemyTextBoxes();
        // HandleScoreChanged
        // HandleCharIdChanged

    }

    // subscribed handleChanges
    private void HandleCharIdChanged(int charId){
        if (charId == -1){
            BlankPlayerTextBoxes(); 
        } else if (charId >= 0 && charId < Instance.charArray.Length){
            var charUnitStats = Instance.charArray[charId].GetComponent<CharacterStats>(); 
            playerUnitTextComponents[0].text = charUnitStats.charName;
            playerUnitTextComponents[1].text = "HP: " + charUnitStats.hp.ToString(); 
            playerUnitTextComponents[2].text = "Attack: " + charUnitStats.attack.ToString();
        } else {
            BlankPlayerTextBoxes();
        }
    }

    private void HandleEnemyIdChanged(int enemyId){
        Debug.Log("initiating handleEnemyIdChnaged");
        if (enemyId == -1){
            BlankEnemyTextBoxes(); 
        } else if (enemyId >= 0 && enemyId < Instance.charArray.Length){
            var charUnitStats = Instance.charArray[enemyId].GetComponent<CharacterStats>(); 
            enemyUnitTextComponents[0].text = charUnitStats.charName;
            enemyUnitTextComponents[1].text = "HP: " + charUnitStats.hp.ToString(); 
            enemyUnitTextComponents[2].text = "Attack: " + charUnitStats.attack.ToString();
        } else {
            BlankEnemyTextBoxes();
        }
    }

    private void BlankPlayerTextBoxes(){
        foreach (TextMeshProUGUI textObj in playerUnitTextComponents){
            textObj.text = "";
        }
    }

    private void BlankEnemyTextBoxes(){
        foreach (TextMeshProUGUI textObj in enemyUnitTextComponents){
            textObj.text = "";
        }
    }



    private void OnDestroy()
    {
        // Unsubscribe from the event when this object is destroyed
        MoveController.OnSelectedCharIdChanged -= HandleCharIdChanged;
        MoveController.OnSelectedEnemyIdChanged -= HandleEnemyIdChanged;
    }


    void Update()
    {
    }


}



/// do i progrmamatically build an array of all the text fields and then 
/// build the components? 
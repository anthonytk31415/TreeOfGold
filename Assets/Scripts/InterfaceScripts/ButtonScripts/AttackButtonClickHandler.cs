using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackButtonClickHandler : MonoBehaviour
{
    [SerializeField] GameManager Instance; 
    [SerializeField] Button attackButton; 

    private void Start() {
        MoveController.OnSelectedEnemyIdChanged += HandleEnemyIdChanged;
    }
    private void Update() {

    }

    public void Exit(){
        MoveController.OnSelectedEnemyIdChanged -= HandleEnemyIdChanged;
    }

    public void HandleEnemyIdChanged(int enemyId){
        if (CanAttackCommence()){
            attackButton.interactable = true;
        } else {
            attackButton.interactable = false;
        }
    }

    public Boolean CanAttackCommence(){
        return Instance.moveControllerObject.GetComponent<MoveController>().IsSelected() && 
            Instance.moveControllerObject.GetComponent<MoveController>().IsEnemySelected();
    }


    // This function will be called when the button is clicked
    public void OnButtonClick()
    {
        Debug.Log("Attack Button Clicked!");
        if (CanAttackCommence())
        {
            /// can possibly split this up into a function so that you can call it with non button actions
            int selectedId = Instance.moveControllerObject.GetComponent<MoveController>().SelectedId; 
            int selectedEnemyId = Instance.moveControllerObject.GetComponent<MoveController>().SelectedEnemyId; 
            GameObject player =  Instance.charArray[selectedId];
            GameObject enemy =  Instance.charArray[selectedEnemyId];
            if (!player.GetComponent<CharacterGameState>().HasAttacked){
                player.GetComponent<CharacterBattle>().CommenceBattle(selectedId, selectedEnemyId, Instance);
                player.GetComponent<CharacterGameState>().HasAttacked = true; 
            }
        }     
    }



}

// if you have attacked, you can be selected to look at your screen 
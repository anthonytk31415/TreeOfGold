using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackButtonClickHandler : MonoBehaviour
{
    [SerializeField] GameManager Instance; 
    [SerializeField] Board board; 

    // This function will be called when the button is clicked
    public void OnButtonClick()
    {
        Debug.Log("Attack Button Clicked!");
        // Add your custom functionality here
        if (Instance.moveControllerObject.GetComponent<MoveController>().IsSelected() && 
            Instance.moveControllerObject.GetComponent<MoveController>().IsEnemySelected())
        {
            // Debug.Log("id's: " + Instance.moveControllerObject.GetComponent<MoveController>().SelectedId +"; " + 
            //             Instance.moveControllerObject.GetComponent<MoveController>().SelectedEnemyId);
                        
            int selectedId = Instance.moveControllerObject.GetComponent<MoveController>().SelectedId; 
            int selectedEnemyId = Instance.moveControllerObject.GetComponent<MoveController>().SelectedEnemyId; 
            GameObject player =  Instance.charArray[selectedId];
            GameObject enemy =  Instance.charArray[selectedEnemyId];
            // Debug.Log("initiating commence attack..");

            if (!player.GetComponent<CharacterGameState>().HasAttacked){
                CharacterBattle.CommenceBattle(selectedId, selectedEnemyId, Instance, board);
            }
        }     
    }
}

// if you have attacked, you can be selected to look at your screen 
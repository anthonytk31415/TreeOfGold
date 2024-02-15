using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackButtonClickHandler : MonoBehaviour
{
    [SerializeField] GameManager Instance; 
    [SerializeField] Board board; 
    // public 
    // This function will be called when the button is clicked
    public void OnButtonClick()
    {
        Debug.Log("Attack Button Clicked!");
        // Add your custom functionality here
        if (Instance.moveControllerObject.GetComponent<MoveController>().IsSelected()){
            int charId = Instance.moveControllerObject.GetComponent<MoveController>().SelectedId; 
            GameObject unit =  Instance.charArray[charId];
            unit.GetComponent<CharacterMove>().PossibleAttackTargets();

        } 
        

    }
}
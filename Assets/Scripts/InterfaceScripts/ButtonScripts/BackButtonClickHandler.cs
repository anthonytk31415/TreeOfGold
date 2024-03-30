using UnityEngine;
using UnityEngine.UI;

public class BackButtonClickHandler : MonoBehaviour
{
    [SerializeField] GameManager Instance; 
    // [SerializeField] Board board; 
    // public 
    // This function will be called when the button is clicked
    public void OnButtonClick()
    {
        // Debug.Log("Button Clicked!");
        // Add your custom functionality here
        MoveController mc = Instance.moveControllerObject.GetComponent<MoveController>();
        mc.UndoMove();
        mc.playerMoveControllerStateMachine.TransitionTo(mc.playerMoveControllerStateMachine.selectedUnmovedState); 
        Instance.highlightTilesManager.TriggerSelectedHighlights();
    }
}
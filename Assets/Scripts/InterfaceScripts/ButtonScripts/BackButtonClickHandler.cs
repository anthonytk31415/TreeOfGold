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
        Instance.moveControllerObject.GetComponent<MoveController>().UndoMove();
        Instance.highlightTilesManager.TriggerSelectedHighlights();
    }
}
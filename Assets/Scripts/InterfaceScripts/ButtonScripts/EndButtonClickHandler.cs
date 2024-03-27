using UnityEngine;
using UnityEngine.UI;

public class EndButtonClickHandler : MonoBehaviour
{
    [SerializeField] GameManager Instance; 

    public void OnButtonClick()
    {
        // Debug.Log("end button clicked");
        Instance.cursorStateMachine.chooseState.endTurn = true;
    }
}
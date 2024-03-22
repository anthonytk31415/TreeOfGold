// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitButton : MonoBehaviour
{

    [SerializeField] GameManager Instance; 
    // [SerializeField] Board board; 
    [SerializeField] Button waitButton; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnButtonClick(){
        int selectedId = Instance.moveControllerObject.GetComponent<MoveController>().SelectedId;   
        // call get cell here on w 
        Coordinate w = Instance.board.FindCharId(selectedId);
        Instance.charArray[selectedId].GetComponent<CharacterMove>().MoveChar(w);       // move the character to wherever it is. 
        Instance.charArray[selectedId].GetComponent<CharacterGameState>().HasAttacked = true;      
        Instance.charArray[selectedId].GetComponent<CharacterGameState>().HasMoved = true; 
        Instance.charArray[selectedId].GetComponent<CharacterGameState>().PerformedAction = true;
        Instance.moveControllerObject.GetComponent<MoveController>().ResetSelected();

    }
}

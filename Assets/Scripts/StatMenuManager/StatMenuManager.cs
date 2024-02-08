using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class StatMenuManager : MonoBehaviour
{

    [SerializeField] GameManager Instance; 
    [SerializeField] Board board; 
    [SerializeField] Text textName; 

    // call this when a unit is selected 

    void Start() {
        GameManager.OnSelectedCharIdChanged += HandleScoreChanged;
    }

    // subscribed handleChanges
    private void HandleScoreChanged(int charId){
        Debug.Log("StatMenu (score placeholder) has changed! New Score: " + charId);
        if (charId == -1){
            textName.text = "Name: "; 
        } else if (charId >= 0 && charId < Instance.charArray.Length){
            GameObject charUnit = Instance.charArray[charId]; 
            string charName = charUnit.GetComponent<CharacterStats>().charName; 
            textName.text = charName; 
        } else {
            textName.text = "Name: ";
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event when this object is destroyed
        GameManager.OnSelectedCharIdChanged -= HandleScoreChanged;
    }


    void Update()
    {
    }


}



/// do i progrmamatically build an array of all the text fields and then 
/// build the components? 
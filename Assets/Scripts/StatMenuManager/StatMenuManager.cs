using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
// using TextMeshPro; 
using TMPro; 
public class StatMenuManager : MonoBehaviour
{

    [SerializeField] GameManager Instance; 
    [SerializeField] Board board; 
    [SerializeField] TextMeshProUGUI textName; 

    // call this when a unit is selected 

    void Start() {
        GameManager.OnSelectedCharIdChanged += HandleCharIdChanged;
        // HandleScoreChanged
        // HandleCharIdChanged
        
    }

    // subscribed handleChanges
    private void HandleCharIdChanged(int charId){
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
        GameManager.OnSelectedCharIdChanged -= HandleCharIdChanged;
    }


    void Update()
    {
    }


}



/// do i progrmamatically build an array of all the text fields and then 
/// build the components? 
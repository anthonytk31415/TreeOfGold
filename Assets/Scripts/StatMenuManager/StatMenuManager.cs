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
    // [SerializeField] TextMeshProUGUI textName; 
    // [SerializeField] TextMeshProUGUI textHp; 
    // [SerializeField] TextMeshProUGUI textAttack; 
    // call this when a unit is selected 
    [SerializeField] TextMeshProUGUI[] textComponents; 



    void Start() {
        GameManager.OnSelectedCharIdChanged += HandleCharIdChanged;
        BlankTextBoxes();
        // HandleScoreChanged
        // HandleCharIdChanged
        
    }

    // subscribed handleChanges
    private void HandleCharIdChanged(int charId){
        if (charId == -1){
            BlankTextBoxes(); 
        } else if (charId >= 0 && charId < Instance.charArray.Length){
            var charUnitStats = Instance.charArray[charId].GetComponent<CharacterStats>(); 
            textComponents[0].text = charUnitStats.charName;
            textComponents[1].text = "HP: " + charUnitStats.hp.ToString(); 
            textComponents[2].text = "Attack: " + charUnitStats.attack.ToString();

        } else {
            BlankTextBoxes();
        }
    }

    private void BlankTextBoxes(){
        foreach (TextMeshProUGUI textObj in textComponents){
            textObj.text = "";
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
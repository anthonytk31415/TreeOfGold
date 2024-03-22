using System.Collections;
using System.Collections.Generic;
using Codice.Client.BaseCommands.Import;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UI;


public class HealthBarManager : MonoBehaviour
{
    [SerializeField] private Slider thisSlider; 

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("health bar starting...");
        InitializeSlider();  
    }

    void InitializeSlider(){
        Debug.Log("slider: " + thisSlider);
        thisSlider.value = 1f ; 
    }

    public IEnumerator UpdateHealthBar(GameObject player){
        Debug.Log("Change initiated for UpdateHealthBar");
        CharacterGameState charGameState = player.GetComponent<CharacterGameState>(); 
        Debug.Log("health pct: " + charGameState.curHp +", " + charGameState.totalHp);
        thisSlider.value = ((float)charGameState.curHp / charGameState.totalHp); 
        yield return new WaitForSeconds(0.15f);

    }


    // Update is called once per frame
    void Update()
    {
        
    }


}

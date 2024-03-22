using System.Collections;
using System.Collections.Generic;
using Codice.Client.BaseCommands.Import;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UI;


// we use HeathBarManager to first attach a prefab onto the character during 
// instantiation via CharactersObject. Then we use the CharacterBattle, which 
// orchestrates battle sequences, whose methods call on methods here to 
// change the HealthBar of a character when the character is damaged. 
// 
// Example call: 
// enemy.GetComponentInChildren<HealthBarManager>().UpdateHealthBar(enemy)
// Notes: 
// - we use GetComponentInChildren since this HealthBar is a child of the char
// - the UpdateHealthBar uses an IEnumerator to chaining these promise-like
//   async functions


public class HealthBarManager : MonoBehaviour
{
    [SerializeField] private Slider thisSlider; 

    // Start is called before the first frame update
    void Start()
    {
        InitializeSlider();  
    }

    void InitializeSlider(){
        thisSlider.value = 1f ; 
    }

    public IEnumerator UpdateHealthBar(GameObject player){
        CharacterGameState charGameState = player.GetComponent<CharacterGameState>(); 
        thisSlider.value = ((float)charGameState.curHp / charGameState.totalHp); 
        yield return new WaitForSeconds(0.15f);
    }


    // Update is called once per frame
    void Update()
    {
        
    }


}

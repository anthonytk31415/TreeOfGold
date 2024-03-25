using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the panel is the lower action bar that gives the player commands to the selected unit. 
// When a char is, you can give additional commands to your unit. 

// if selected ID != -1 and has not taken action

public class SelectedPanel : MonoBehaviour
{

    public GameObject selectedPanel; 

    // Start is called before the first frame update
    void Start()
    {
        selectedPanel.SetActive(false); 
        MoveController.OnSelectedCharIdChanged += HandleCharIdChanged; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HandleCharIdChanged(int selectedId){
        if (selectedId != -1){
            selectedPanel.SetActive(true); 
        } else {
            selectedPanel.SetActive(false); 
        }
    }

    public void Exit(){
        MoveController.OnSelectedCharIdChanged -= HandleCharIdChanged;
    }
}

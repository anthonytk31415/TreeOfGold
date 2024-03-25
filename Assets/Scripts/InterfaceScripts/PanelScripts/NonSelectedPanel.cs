using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the panel is the lower action bar that gives the player commands to the selected unit. 
// When nothing is selected, this is defaulted, all you can do is basically end turn


public class NonSelectedPanel : MonoBehaviour
{
    public GameObject nonSelectedPanel; 

    // Start is called before the first frame update
    void Start()
    {
        nonSelectedPanel.SetActive(true); 
        MoveController.OnSelectedCharIdChanged += HandleCharIdChanged; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HandleCharIdChanged(int selectedId){
        if (selectedId == -1){
            nonSelectedPanel.SetActive(true); 
        } else {
            nonSelectedPanel.SetActive(false); 
        }
    }

    public void Exit(){
        MoveController.OnSelectedCharIdChanged -= HandleCharIdChanged;
    }
}

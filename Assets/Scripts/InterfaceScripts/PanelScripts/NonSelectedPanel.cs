using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// default panel 


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

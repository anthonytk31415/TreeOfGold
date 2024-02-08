using System;
using System.Collections; 
using System.Collections.Generic; 
using UnityEngine;

// we have a tile prefab and use the Unity editor to bid the Tile.cs script to the 
// prefab itself. We then use the Init() method to iteratively build our m x n grid. 

public class Tile : MonoBehaviour {
    [SerializeField] private Color _baseColor, _offsetColor, _menuColor; 
    [SerializeField] private SpriteRenderer _renderer;

    [SerializeField] private GameObject _highlight;
    [SerializeField] private Color _playerPathColor;
    private Boolean isOnBoard {get; set; }
    public void Init(bool isOffset, int y ) {
        _renderer.color = isOffset ? _offsetColor : _baseColor;
        if (y >= 9 || y == 0) {
            isOnBoard = false; 
            _renderer.color = _menuColor;
        }
        else {
            isOnBoard = true;
        }
    }

    public void TogglePlayerPath(){
        _renderer.color = _playerPathColor; 
    }


    // Mouse Hover Properties
    void OnMouseEnter() {
        if (isOnBoard) {
            _highlight.SetActive(true);
        }

    }
 
    void OnMouseExit(){
        if (isOnBoard) {
        _highlight.SetActive(false);
        }
    }
}
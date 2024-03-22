using System;
using System.Collections; 
using System.Collections.Generic; 
using UnityEngine;

// we have a tile prefab and use the Unity editor to bind the Tile.cs script to the 
// prefab itself. We then use the Init() method to iteratively build our m x n grid. 

public class Tile : MonoBehaviour {
    [SerializeField] private Color _baseColor, _offsetColor, _menuColor; 
    [SerializeField] private Color _playerAllPathColor, _playerPathColor, _playerColor;
    [SerializeField] private Color _enemyAllPathColor, _enemyPathColor, _enemyColor, _enemyTarget;
    [SerializeField] private SpriteRenderer _renderer;

    [SerializeField] private GameObject _highlight;

    public Boolean isOnBoard {get; set; }
    private Boolean isOffsetColor {get; set; } 
    public void Init(bool isOffset, int y ) {

        // _renderer.color = isOffset ? _offsetColor : _baseColor;
        if (y >= 9 || y == 0) {
            isOnBoard = false; 
            _renderer.color = _menuColor;
        }
        else {
            isOffsetColor = isOffset; 
            isOnBoard = true;
            PaintOffsetColor(); 
        }
    }


    public void PaintOffsetColor(){
        _renderer.color = isOffsetColor ? _offsetColor : _baseColor;
    }

    public void TogglePlayerAllPath(){
        _renderer.color = _playerAllPathColor; 
    }

    public void TogglePlayerPath(){
        _renderer.color = _playerPathColor; 
    }

    public void TogglePlayer(){
        _renderer.color = _playerColor; 
    }

    public void ToggleEnemy(){
        _renderer.color = _enemyColor; 
    }

    public void ToggleEnemyTarget(){
        _renderer.color = _enemyTarget; 
    }


    // Mouse Hover Properties
    // void OnMouseEnter() {
    //     if (isOnBoard) {
    //         _highlight.SetActive(true);
    //     }

    // }
 
    // void OnMouseExit(){
    //     if (isOnBoard) {
    //     _highlight.SetActive(false);
    //     }
    // }
}
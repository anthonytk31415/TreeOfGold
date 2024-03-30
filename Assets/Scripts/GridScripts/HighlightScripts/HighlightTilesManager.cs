using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HighlightTilesManager
{
    private GameManager instance;
    private Board board; 
    public HighlightTilesManager(GameManager instance){
        this.instance = instance;
        this.board = instance.board;  
    }

    /// <summary>
    /// Governs the tile highlights based on what units are selected.
    /// Currently, only implemented for player phase.
    /// This is typically triggered by listening to MoveController's state (selectedId, selectedEnemyId),
    /// or by making actions on the player phase (see the PlayerState event listener subscription).
    /// </summary>
    public void TriggerSelectedHighlights(){
        int selectedId = instance.moveControllerObject.GetComponent<MoveController>().SelectedId;
        int enemyId =  instance.moveControllerObject.GetComponent<MoveController>().SelectedEnemyId;     
        ResetTiles();        
        if (selectedId != -1 ){
            GameObject selectedUnit = instance.charArray[selectedId]; 
            HighlightAllPlayerMoves();
            if (!selectedUnit.GetComponent<CharacterGameState>().HasMoved){
                HighlightPlayerUnitMoves(selectedId);    
            }
            HighlightUnit(selectedId);
            if (!selectedUnit.GetComponent<CharacterGameState>().HasAttacked){
                HighlightValidTargets(selectedId);
            }
        } else if (selectedId == -1 && enemyId != -1) {
            HighlightUnit(enemyId); 
        }
    }

    // 
    /// <summary>
    /// Get all moves for those chars on your team and has not moved. 
    /// Use this method when you first click on a player unit and after  
    /// a move is completed.
    /// </summary>
    public void HighlightAllPlayerMoves(){
        // 
        HashSet<Coordinate> allPlayerMoves = new(); 
        foreach (GameObject character in instance.charArray) {
            if (character.GetComponent<CharacterGameState>().isYourTeam && !character.GetComponent<CharacterGameState>().HasMoved && character.GetComponent<CharacterGameState>().IsAlive){
                HashSet<Coordinate> curMoves = character.GetComponent<CharacterMove>().PossibleMoves();
                allPlayerMoves.UnionWith(curMoves);
            }
        }

        // highlight those moves on the grid
        foreach (Tile tile in instance.tiles){            
            HighlightTile(tile, allPlayerMoves, tile.TogglePlayerAllPath);
        }
    }

    /// <summary>
    /// Highlight the individual moves of a unit.
    /// </summary>
    /// <param name="charId"> an int that is the Id of char</param>
    public void HighlightPlayerUnitMoves(int charId){
        GameObject unit = instance.charArray[charId]; 
        HashSet<Coordinate> curMoves = unit.GetComponent<CharacterMove>().PossibleMoves();
        foreach (Tile tile in instance.tiles){            
            HighlightTile(tile, curMoves, tile.TogglePlayerPath);
        }
    }


    /// <summary>
    /// Method that, given a tile, determines if that tile is a part of the set of moves, and if so, highlights that specific tile. Used 
    /// to mark where you're going to set the unit.
    /// </summary>
    /// <param name="tile">the ui Tile </param>
    /// <param name="setOfMoves">A hashset of a specific player moves</param>
    /// <param name="tileHighlight"> pass in a method, typically from the tile class for a specific tile, that will change its color </param>
    public void HighlightTile(Tile tile, HashSet<Coordinate> setOfMoves, Action tileHighlight){
        Transform targetTransform = tile.transform;
        float xPosition = targetTransform.position.x;
        float yPosition = targetTransform.position.y;
        Coordinate w = board.ConvertSceneToMatCoords((double)xPosition, (double)yPosition); 
        if (setOfMoves.Contains(w)){
            tileHighlight();
        }   
    }


    /// <summary>
    /// Highlights the charId based on whether the charId is player or enemy
    /// </summary>
    /// <param name="charId">charId id used to determine location of char on the board to highlight.</param>
    public void HighlightUnit(int charId){
        Coordinate w = board.FindCharId(charId);
        Boolean yourTeam = instance.charArray[charId].GetComponent<CharacterGameState>().isYourTeam; 
        if (board.IsNull(w)){
            return; 
        } else {
            Tile tile = GridManager.FindTile(instance.tiles, w); 
            if (yourTeam){
                tile.TogglePlayer();
            }
            else {
                tile.ToggleEnemy(); 
            }
        }
    }

    /// <summary>
    /// given a charId, get the available attacks, then for each thing in the hash set, highlight 
    /// </summary>
    /// <param name="selectedUnitId"></param>
    public void HighlightValidTargets(int selectedUnitId){
        // Coordinate w = board.FindCharId(charId);
        GameObject selectedUnit = instance.charArray[selectedUnitId]; 
        HashSet<Coordinate> curEnemyTargets = selectedUnit.GetComponent<CharacterMove>().PossibleAttackTargets();

        // highlight those moves on the grid
        foreach (Tile tile in instance.tiles){            
            HighlightTile(tile, curEnemyTargets, tile.ToggleEnemyTarget);
        }
    }

    /// <summary>
    ///  Reset all of the board tiles to its offset color
    /// </summary>
    public void ResetTiles(){
        foreach (Tile tile in instance.tiles){            
            if (tile.isOnBoard){
                tile.PaintOffsetColor(); 
            }
        }
    }

}

using System; 
using System.Collections;
using System.Collections.Generic;
using PlasticPipe.PlasticProtocol.Messages;
using UnityEngine;

// here we choose the 


public class ChooseState : ICursorState
{
    public GameObject cursor; 
    public GameManager instance; 
    public Board board; 

    // some properties that determine what the cursor does

    public ChooseState(GameObject cursor, GameManager instance, Board board) {
        this.cursor = cursor;
        this.instance = instance;
        this.board = board;  
    }


    // trigger all the things you want to do when you enter
    public void Enter(){
        HighlightAllPlayerMoves();
        MoveController.OnSelectedCharIdChanged += HandleCharIdChanged; 
    }

    public void Update(){
    }

    public void Exit(){
        MoveController.OnSelectedCharIdChanged -= HandleCharIdChanged;
    }

    // lots of move choices below; do we reorganize later into its own move class?

    public void HandleCharIdChanged(int charId){
        TriggerSelectedHighlights();
    }


    // highlights are defined by selected units. 
    // if selectedId != -1 then 
    // selectedId == -1 and enemyId != 01

    public void TriggerSelectedHighlights(){
        int selectedId = instance.moveControllerObject.GetComponent<MoveController>().SelectedId;
        int enemyId =  instance.moveControllerObject.GetComponent<MoveController>().SelectedEnemyId;        
        ResetBoard();        
        if (selectedId != -1 ){
            HighlightUnit(selectedId);
            HighlightPlayerUnitMoves(selectedId);    
            HighlightValidTargets(selectedId);
            
        } else if (selectedId == -1 && enemyId != -1) {
            HighlightUnit(enemyId); 
        }

    }
    


    public void ResetBoard(){
        ResetTiles();
        HighlightAllPlayerMoves();
    }

    // call this when you begin and after you've donea move(probably a switch in state so no need to redo call)
    public void HighlightAllPlayerMoves(){
        // Get all moves for those chars on your team and has not moved
        HashSet<Coordinate> allPlayerMoves = new(); 
        foreach (GameObject character in instance.charArray) {
            if (character.GetComponent<CharacterGameState>().isYourTeam && !character.GetComponent<CharacterGameState>().HasMoved){
                HashSet<Coordinate> curMoves = character.GetComponent<CharacterMove>().PossibleMoves();
                allPlayerMoves.UnionWith(curMoves);
            }
        }

        // highlight those moves on the grid
        foreach (Tile tile in instance.tiles){            
            HighlightTile(tile, allPlayerMoves, tile.TogglePlayerAllPath);
        }
    }
    public void HighlightPlayerUnitMoves(int charId){
        GameObject unit = instance.charArray[charId]; 
        HashSet<Coordinate> curMoves = unit.GetComponent<CharacterMove>().PossibleMoves();
        foreach (Tile tile in instance.tiles){            
            HighlightTile(tile, curMoves, tile.TogglePlayerPath);
        }
    }

    public void HighlightTile(Tile tile, HashSet<Coordinate> setOfMoves, Action tileHighlight){
        Transform targetTransform = tile.transform;
        float xPosition = targetTransform.position.x;
        float yPosition = targetTransform.position.y;
        Coordinate w = board.ConvertSceneToMatCoords((double)xPosition, (double)yPosition); 
        if (setOfMoves.Contains(w)){
            tileHighlight();
        }   
    }

    public void HighlightUnit(int charId){
        Coordinate w = board.FindCharId(charId);
        Boolean yourTeam = instance.charArray[charId].GetComponent<CharacterGameState>().isYourTeam; 
        Tile tile = GridManager.FindTile(instance.tiles, w); 
        if (yourTeam){
            tile.TogglePlayer();
        }
        else {
            tile.ToggleEnemy(); 
        }
    }


    // given a charId, get the available attacks, then for each thing in the hash set, highlight
    // dont forget to include whatever you need in the reset tiles

    public void HighlightValidTargets(int selectedUnitId){
        // Coordinate w = board.FindCharId(charId);
        GameObject selectedUnit = instance.charArray[selectedUnitId]; 
        HashSet<Coordinate> curEnemyTargets = selectedUnit.GetComponent<CharacterMove>().PossibleAttackTargets();

        // highlight those moves on the grid
        foreach (Tile tile in instance.tiles){            
            HighlightTile(tile, curEnemyTargets, tile.ToggleEnemyTarget);
        }
    }


    public void ResetTiles(){
        foreach (Tile tile in instance.tiles){            
            if (tile.isOnBoard){
                tile.PaintOffsetColor(); 
            }
        }
    }


}
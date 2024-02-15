using System; 
using System.Collections;
using System.Collections.Generic;
using PlasticPipe.PlasticProtocol.Messages;
using UnityEngine;

// here we choose the 


public class ChooseState : ICursorState
{
    public GameObject cursor; 
    public GameManager gameManager; 
    public Board board; 

    // some properties that determine what the cursor does

    public ChooseState(GameObject cursor, GameManager gameManager, Board board) {
        this.cursor = cursor;
        this.gameManager = gameManager;
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
        TriggerSelectedHighlights(charId);
    }

    public void TriggerSelectedHighlights(int charId){
        ResetBoard();
        if (charId != -1 ){
            HighlightUnit(charId);
        }
        if (charId != -1 && gameManager.charArray[charId].GetComponent<CharacterGameState>().isYourTeam){
            HighlightPlayerUnitMoves(charId);    
            HighlightValidTargets(charId);        
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
        foreach (GameObject character in gameManager.charArray) {
            if (character.GetComponent<CharacterGameState>().isYourTeam && !character.GetComponent<CharacterGameState>().HasMoved){
                HashSet<Coordinate> curMoves = character.GetComponent<CharacterMove>().PossibleMoves();
                allPlayerMoves.UnionWith(curMoves);
            }
        }

        // highlight those moves on the grid
        foreach (Tile tile in gameManager.tiles){            
            HighlightTile(tile, allPlayerMoves, tile.TogglePlayerAllPath);
        }
    }
    public void HighlightPlayerUnitMoves(int charId){
        GameObject unit = gameManager.charArray[charId]; 
        HashSet<Coordinate> curMoves = unit.GetComponent<CharacterMove>().PossibleMoves();
        foreach (Tile tile in gameManager.tiles){            
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
        Boolean yourTeam = gameManager.charArray[charId].GetComponent<CharacterGameState>().isYourTeam; 
        Tile tile = GridManager.FindTile(gameManager.tiles, w); 
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
        GameObject selectedUnit = gameManager.charArray[selectedUnitId]; 
        HashSet<Coordinate> curEnemyTargets = selectedUnit.GetComponent<CharacterMove>().PossibleAttackTargets();

        // highlight those moves on the grid
        foreach (Tile tile in gameManager.tiles){            
            HighlightTile(tile, curEnemyTargets, tile.ToggleEnemyTarget);
        }
    }


    public void ResetTiles(){
        foreach (Tile tile in gameManager.tiles){            
            if (tile.isOnBoard){
                tile.PaintOffsetColor(); 
            }
        }
    }


}
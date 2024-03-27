using System; 
using System.Collections.Generic;
using UnityEngine;

// CharInteraction handles possible movement positions  
// Do I call this CharStatePosition? 

public static class CharInteraction 
{
    // given board, distance d, and current position w, returns set of all possible 
    // moves. Can be used for movement, ranged attacks from 0-d. 
    public static HashSet<Coordinate> SetOfMoves(GameManager gameManager, Coordinate initialPos, int dist){
        Board board = gameManager.board;
        HashSet<Coordinate> res = new();           
        Queue<(Coordinate, int)> queue = new();
        queue.Enqueue((initialPos, dist));
        res.Add(initialPos);

        while (queue.Count > 0){
            (Coordinate curPos, int curMove) = queue.Dequeue(); 
            (int, int)[] positions = {(1,0), (-1, 0), (0, 1), (0, -1)}; 
            if (curMove > 0) {
                foreach ((int dx, int dy) in positions) 
                {
                    Coordinate w = new(curPos.GetX() + dx, curPos.GetY() + dy);
                    if (!res.Contains(w) && board.IsWithinBoard(w) && board.IsEmpty(w))
                    {
                        res.Add(w);
                        queue.Enqueue((w, curMove - 1)); 
                    }
                }
            }
        }
        return res; 
    } 

    // given an initial position of a character (who is on that position), along with dist to search (i.e. )
    // the units move set, return the set of all coordinates the unit can move to
    public static HashSet<Coordinate> PlayerMoveOptions(Coordinate initialPos, int dist, GameManager gameManager){
        Board board = gameManager.board; 
        GameObject character = gameManager.charArray[board.Get(initialPos)];
        bool charIsYourTeam = character.GetComponent<CharacterGameState>().isYourTeam; 
        HashSet<Coordinate> res = new();
        Queue<(Coordinate, int)> queue = new();
        HashSet<Coordinate> visited = new(); 
        res.Add(initialPos);           
        queue.Enqueue((initialPos, dist));
        visited.Add(initialPos);

        while (queue.Count > 0){
            (Coordinate curPos, int curMove) = queue.Dequeue(); 
            (int, int)[] positions = {(1,0), (-1, 0), (0, 1), (0, -1)}; 
            if (curMove > 0) {
                foreach ((int dx, int dy) in positions) 
                {
                    Coordinate w = new(curPos.GetX() + dx, curPos.GetY() + dy);
                    if (!visited.Contains(w) && board.IsWithinBoard(w)){
                        // units can "go through" friendly units, but not opp. team units
                        if (board.IsEmpty(w)){
                            res.Add(w);
                            visited.Add(w);
                            queue.Enqueue((w, curMove - 1));
                        }
                        else {
                            GameObject unit = gameManager.charArray[board.Get(w)];
                            bool isTeammate = unit.GetComponent<CharacterGameState>().isYourTeam ;
                            if (isTeammate == charIsYourTeam){
                                visited.Add(w);
                                queue.Enqueue((w, curMove - 1));
                            }
                        }
                    }
                }
            }
        }
        return res; 
    } 

    // given character in a current position (either team), return coordinates of 
    // opposing team within attack range; 
    // coded O(mn) Time
    public static HashSet<Coordinate> EnemiesWithinAttackRange(GameManager gameManager, GameObject character, Coordinate initialPos, int range){
        Board board = gameManager.board;       
        bool charTeam = character.GetComponent<CharacterGameState>().isYourTeam; 
        HashSet<Coordinate> res = new();           
        Queue<(Coordinate, int)> queue = new();
        queue.Enqueue((initialPos, range));
        // res.Add(w);

        while (queue.Count > 0){
            (Coordinate curPos, int curMove) = queue.Dequeue(); 
            (int, int)[] positions = {(1,0), (-1, 0), (0, 1), (0, -1)}; 
            if (curMove > 0) {
                foreach ((int dx, int dy) in positions) 
                {
                    Coordinate w = new(curPos.GetX() + dx, curPos.GetY() + dy);
                    if (!res.Contains(w) && board.IsWithinBoard(w) && board.Get(w) != -1 
                        && (gameManager.charArray[board.Get(w)].GetComponent<CharacterGameState>().isYourTeam != charTeam) )
                    {
                        res.Add(w);
                        queue.Enqueue((w, curMove - 1)); 
                    }
                }
            }
        }
        return res; 
    }


    // i dont know what i was doing with this...
    public static void AuditEnemiesInRange(HashSet<Coordinate> enemiesInRange){
        // Debug.Log("testing all enemies within range: ");
        foreach(Coordinate w in enemiesInRange){
            Debug.Log(w);
        }
    }

    // given current unit, return 

    // public static int ResultOpponentHPAfterBattle(int unitId, GameManager instance, int targetId){
    //     GameObject character = instance.charArray[unitId];
    //     int charAttack = character.GetComponent<CharacterStats>().attack; 
    //     GameObject opponent = instance.charArray[targetId];
    //     return opponent.GetComponent<CharacterGameState>().HPAfterAttack(charAttack);
    // }


}
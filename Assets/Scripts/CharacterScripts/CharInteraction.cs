using System; 
using System.Collections.Generic;
using System.Linq;
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
        // Debug.Log(gameManager.charArray[0]);
        // Debug.Log("initial pos: " + initialPos +"; selectedID: " + gameManager.moveControllerObject.GetComponent<MoveController>().SelectedId);
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

    public static void AuditEnemiesInRange(HashSet<Coordinate> enemiesInRange){
        string debugMsg = ""; 
        foreach(Coordinate w in enemiesInRange){
            debugMsg += w; 
        }
        Debug.Log(debugMsg);
    }

    // return the move that puts you to the closest unit
    public static Coordinate MoveToClosestEnemy(Coordinate posChar){
        return new Coordinate(-1, -1);
    }


// you cant move to the final char2. 


    /// <summary>
    /// Returns the shortest path the the char2 from char1 with rules on being
    /// able to not occupy a friendly cell before the char2, but being able to 
    /// walk through a friendly (but not through an enemy). 
    /// Note ** the last element of the list is the unit itself. 
    /// Testing whether we can use char2 as a generic moving cell
    /// Update: will rename this P
    /// </summary>
    /// <param name="gameManager"> the singleton instance gameManager</param>
    /// <param name="char1">starting point w, typically the player unit </param>
    /// <param name="char2">ending point v, sometimes the target enemy unit</param>
    /// <returns></returns>
    public static List<Coordinate> ShortestPathBetweenCoordinates(GameManager gameManager, Coordinate char1, Coordinate char2){
        Dictionary<Coordinate, Coordinate> parent = new();          // parents of (-1, -1) have no parent
        Queue<Coordinate> queue = new();
        HashSet<Coordinate> visited = new();
        queue.Enqueue(char1);         
        visited.Add(char1);
        parent.Add(char1, new Coordinate(-1, -1));
        (int, int)[] positions = {(1,0), (-1, 0), (0, 1), (0, -1)}; 

        bool foundPath = false; 

        // use BFS to get shortest path to char2 from char1 using the queue
        // we adjust this so that if we dont find a path, we return an empty queue
        while (queue.Count > 0){
            Coordinate curPos = queue.Dequeue();
            if (curPos.Equals(char2)){          
                foundPath = true;       
                break;
            }
            foreach ((int dx, int dy) in positions) 
            {
                Coordinate newPos = new Coordinate(curPos.GetX() + dx, curPos.GetY() + dy);
                // not in visited, within board, not enemy, and if friend, not adjacent to target
                // you cannot go to char2 if you are currently on a friendly unit that's not you yourself
                // but you can go to a friendly unit
                if (gameManager.board.IsWithinBoard(newPos) && 
                        !visited.Contains(newPos) &&
                        (gameManager.moveControllerObject.GetComponent<MoveController>().IsNotEnemy(char1, newPos) || 
                                newPos.Equals(char2)
                    ))                                        
                {
                    // if cur move is friend and next move is char2:                     
                    visited.Add(newPos);
                    parent.Add(newPos, curPos); 
                    queue.Enqueue(newPos);
                    
                }
            }
        }



        List<Coordinate> res = new();
        // we got to char2 position; now trace back using parent
        if (!foundPath){
            return res; 
        } 

        Coordinate tracePos = char2; 
        while (!tracePos.Equals(char1))
        {
            res.Add(tracePos); 
            tracePos = parent[tracePos];
        }
        res.Reverse();
        return res; 
    }

    /// <summary>
    /// Given two units, we return the shortest path on the top, left, right, or down cell of unit2. 
    /// If the path is infinite, i.e. its blocked completely? then we return an empty list. 
    /// </summary>
    /// <param name="gameManager"></param>
    /// <param name="unit1"></param>
    /// <param name="unit2"></param>
    /// <returns></returns>    
    public static List<Coordinate> ShortestPathBetweenUnits(GameManager gameManager, Coordinate unit1, Coordinate unit2){
        List<Coordinate> candidates = new();
        List<List<Coordinate>> candidatesPaths = new();
        (int, int)[] positions = {(1,0), (-1, 0), (0, 1), (0, -1)}; 

        foreach ((int dx, int dy) in positions) 
        {
            Coordinate possibleCandidate = new Coordinate(unit2.GetX() + dx, unit2.GetY() + dy);
            // not in visited, within board, not enemy, and if friend, not adjacent to target
            // you cannot go to char2 if you are currently on a friendly unit that's not you yourself
            // but you can go to a friendly unit
            if (gameManager.board.IsWithinBoard(possibleCandidate) && gameManager.board.IsEmpty(possibleCandidate))
            {
                candidates.Add(possibleCandidate);
                List<Coordinate> possibleCandidatePath = ShortestPathBetweenCoordinates(gameManager, unit1, possibleCandidate); 
                if (possibleCandidatePath.Count > 0){
                    candidatesPaths.Add(ShortestPathBetweenCoordinates(gameManager, unit1, possibleCandidate));
                }
            }
        }
        candidatesPaths.Sort((x, y) => x.Count.CompareTo(y.Count));
        if (candidatesPaths.Count > 0){
            return candidatesPaths[0];
        }
        return new List<Coordinate>(); 
    }




    /// <summary>
    /// Given a unit based on its coordinate, return the path to the closest
    /// opponent
    /// </summary>
    /// <param name="gameManager"></param>
    /// <param name="unit"></param>
    /// <returns></returns>
    public static List<Coordinate> FindClosestOpponent(GameManager gameManager, Coordinate unit)
    {
        List<(int, List<Coordinate>)> opponentCandidates = new();
        for (int id = 0; id < gameManager.charArray.Length; id ++ )
        {
            Coordinate curUnit = gameManager.board.FindCharId(id);
            GameObject curUnitObject = gameManager.charArray[id]; 

            if (curUnitObject.GetComponent<CharacterGameState>().IsAlive && 
                    !curUnit.Equals(unit) && 
                    gameManager.moveControllerObject.GetComponent<MoveController>().IsEnemy(unit, curUnit))
            {
                List<Coordinate> curPath = ShortestPathBetweenCoordinates(gameManager, unit, curUnit); 
                if (curPath.Count > 0){
                    opponentCandidates.Add((curPath.Count, curPath));
                }
            }
        }
        opponentCandidates.Sort((x, y) => x.Item1.CompareTo(y.Item1));
        if (opponentCandidates.Count > 0){
            return opponentCandidates[0].Item2; 
        }
        return new List<Coordinate>();
    }




    // given current unit, return 

    // public static int ResultOpponentHPAfterBattle(int unitId, GameManager instance, int targetId){
    //     GameObject character = instance.charArray[unitId];
    //     int charAttack = character.GetComponent<CharacterStats>().attack; 
    //     GameObject opponent = instance.charArray[targetId];
    //     return opponent.GetComponent<CharacterGameState>().HPAfterAttack(charAttack);
    // }


}
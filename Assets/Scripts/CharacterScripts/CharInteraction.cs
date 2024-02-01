using System; 
using System.Collections.Generic;
public static class CharInteraction 
{
    // given board, distance d, and current position w, returns set of all possible 
    // moves. Can be used for movement, ranged attacks from 0-d. 
    public static HashSet<Coordinate> SetOfMoves(Board board, Coordinate initialPos, int dist)
        {
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
                        if (!res.Contains(w) && board.IsValidEntry(w) && board.IsEmpty(w))
                        {
                            res.Add(w);
                            queue.Enqueue((w, curMove - 1)); 
                        }
                    }
                }
            }
            return res; 
        } 

}
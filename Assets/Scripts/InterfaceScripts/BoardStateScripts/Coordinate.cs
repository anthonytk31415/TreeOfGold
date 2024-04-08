// represents coordinates for the board
using System;
using System.Collections.Generic;
using System.Numerics;

public class Coordinate {
    public int x; 
    public int y; 

    public Coordinate(int x, int y) {    
        this.x = x;
        this.y = y;
    }

    public int GetX() {
        return x; 
    }

    public int GetY()  {
        return y; 
    }

    public override bool Equals(object obj) {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Coordinate coordObj = (Coordinate)obj;
        return this.GetX() == coordObj.GetX() && this.GetY() == coordObj.GetY();
    }
    // review the mechanics of this Value
    public int Value { get; set; }
    public override int GetHashCode() {
        return Value.GetHashCode();
    }
    public override string ToString() {
        return string.Format("Coordinate with x: {0}, y: {1}", x, y);
    }

    /// <summary>
    /// gives manhattan distance between end and start. 
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public static Vector2 Difference(Coordinate start, Coordinate end){
        int dx = end.GetX() - start.GetX();
        int dy = end.GetY() - start.GetY();
        return new Vector2(dx, dy); 
    }

    /// <summary>
    /// Given adjacent coordinates, return direction you must go from start to get to end.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public static  Direction DirectionFromAdjacentCoordinates(Coordinate start, Coordinate end){
        Dictionary<Vector2, Direction> vectorToDirection = new()
        {
            {new Vector2(0, 1), Direction.up},
            {new Vector2(0, -1), Direction.down},
            {new Vector2(-1, 0), Direction.left},
            {new Vector2(1, 0), Direction.right}
        };
        Vector2 delta = Difference(start, end);
        if (vectorToDirection.ContainsKey(delta)){
            return vectorToDirection[delta];
        }
        throw new Exception("Not adjacent cells");
    }

}

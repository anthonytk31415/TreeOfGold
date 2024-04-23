using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections;
using System.Collections.Generic;


public static class DirectionUtility {
    public static Vector2 DirectionToVector(Direction direction){
        Dictionary<Direction, Vector2> directions  = new()
        {
            {Direction.up, Vector2.up},
            {Direction.down, Vector2.down},
            {Direction.left, Vector2.left},
            {Direction.right, Vector2.right}
        }; 
        return directions[direction];
    }

    public static Coordinate DirectionToCoordinate(Direction direction){
        Dictionary<Direction, Coordinate> directions  = new()
        {
            {Direction.up, new Coordinate(0, 1)},
            {Direction.down, new Coordinate(0, -1)},
            {Direction.left, new Coordinate(-1, 0)},
            {Direction.right, new Coordinate(1, 0)}
        };
        return directions[direction];
    }

    public static string DirectionToIdleBool(Direction direction){
        Dictionary<Direction, string> directions  = new()
        {
            {Direction.up, "idleUpBool" },
            {Direction.down, "idleDownBool"},
            {Direction.left, "idleLeftBool"},
            {Direction.right, "idleRightBool"}
        };
        return directions[direction];
    }

    public static Dictionary<Direction, string> directionToWalkBool = new()
    {
        {Direction.up, "walkUpBool" },
        {Direction.down, "walkDownBool"},
        {Direction.left, "walkLeftBool"},
        {Direction.right, "walkRightBool"}
    };

    public static string DirectionToWalkBool(Direction direction){
        Dictionary<Direction, string> directions  = new()
        {
            {Direction.up, "walkUpBool" },
            {Direction.down, "walkDownBool"},
            {Direction.left, "walkLeftBool"},
            {Direction.right, "walkRightBool"}
        };
        return directions[direction];
    }

    public static string DirectionToAttackSwordTrigger(Direction direction){
        Dictionary<Direction, string> directions  = new()
        {
            {Direction.up, "attackSwordUpTrigger" },
            {Direction.down, "attackSwordDownTrigger"},
            {Direction.left, "attackSwordLeftTrigger"},
            {Direction.right, "attackSwordRightTrigger"}
        };
        return directions[direction];
    }

    public static string DirectionToBlinkBlack(Direction direction){
        Dictionary<Direction, string> directions  = new()
        {
            {Direction.up, "idleBlinkBlackUpTrigger" },
            {Direction.down, "idleBlinkBlackDownTrigger"},
            {Direction.left, "idleBlinkBlackLeftTrigger"},
            {Direction.right, "idleBlinkBlackRightTrigger"}
        };
        return directions[direction];
    }

    public static string DirectionToBlinkRed(Direction direction){
        Dictionary<Direction, string> directions  = new()
        {
            {Direction.up, "idleBlinkRedUpTrigger" },
            {Direction.down, "idleBlinkRedDownTrigger"},
            {Direction.left, "idleBlinkRedLeftTrigger"},
            {Direction.right, "idleBlinkRedRightTrigger"}
        };
        return directions[direction];
    }


    public static Direction OppositeDirection(Direction direction){
        Dictionary<Direction, Direction> directions  = new()
        {
            {Direction.up, Direction.down },
            {Direction.down, Direction.up},
            {Direction.left, Direction.right},
            {Direction.right, Direction.left}
        };
        return directions[direction];
    }
}
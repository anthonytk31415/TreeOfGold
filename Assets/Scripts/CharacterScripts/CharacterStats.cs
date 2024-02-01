using System;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int attack;
    public int hp;
    public int moves; 

    // Other methods as needed
    public String GetStats()
    {
        return "HP: " + hp + "; Attack: " + attack + "Moves: " + moves;
    }

    public void Initialize(int hp, int attack, int moves)
    {
        this.hp = hp; 
        this.attack = attack;
        this.moves = moves;
    }

    public int GetHp()
    {
        // Debug.Log(hp);
        return hp; 
    }
}
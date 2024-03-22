using System;
using UnityEngine;

// CharacterStats purely handles the fixed stats of the character
// Note it does not handle the stat state of the character. 

public class CharacterStats : MonoBehaviour
{
    public string charName; 
    public int attack;
    public int hp;
    public int moves; 
    public int attackRange; 

    // Other methods as needed
    public String GetStats()
    {
        return "HP: " + hp + "; Attack: " + attack + "Moves: " + moves + "Attack Range: " + attackRange;
    }

    public void Initialize(string charName, int hp, int attack, int moves, int attackRange)
    {
        this.charName = charName; 
        this.hp = hp; 
        this.attack = attack;
        this.moves = moves;
        this.attackRange = attackRange;
    }

    public int GetHp()
    {
        // Debug.Log(hp);
        return hp; 
    }
}
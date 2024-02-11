using System.Collections; 
using System.Collections.Generic; 
using UnityEngine;

// this is an array of the references to the tiles on the screen. 
// Tile[x, y] = reference of tile
public class GridManager : MonoBehaviour {
        
    public static Tile[,] Initialize(int width, int height){
        Tile tilePrefab = Resources.Load("Prefabs/Grids/" + "GreenTile", typeof(Tile)) as Tile;            
        Tile[,] tiles = new Tile[width, height]; 
        for (int x = 0; x < width; x ++){
            for (int y = 0; y < height; y ++){
                Tile spawnedTile = Instantiate(tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x}, {y}";                 

                // build checkered pattern on tile boolean 
                var isOffset = (x % 2 == 0 && y %2 != 0)  || (x % 2 != 0 && y %2 == 0);
                spawnedTile.Init(isOffset, y);
                tiles[x,y] = spawnedTile; 
            }
        }

        return tiles; 
    }

    // currently assumes the bottom row is the status bar so we just add 1 to the x coordinate
    // will probably need to refactor later 
    public static Tile FindTile(Tile[,] tiles, Coordinate w){
        return tiles[w.GetX(), w.GetY() + 1];
    }
}
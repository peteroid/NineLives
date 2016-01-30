using UnityEngine;
using System.Collections;

public class TileSystem : MonoBehaviour {

    /*
     * Tiles are as follows:
     * 0 - passable
     * 1 - impassable
     */

    public bool passable;
    public int[][] tileMapTest = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                                   1, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                                   1, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                                   1, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                                   1, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                                   1, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                                   1, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                                   1, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                                   1, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                                   1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

    public void GenerateTileMap()
    { 
        // Instantiate the tile types onto the game world, or placing them - KTZ
    }

    public void GenerateCollision()
    { 
        // Put collision on any tiles that are impassible - KTZ
    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;

public class TileSystem : MonoBehaviour {

    /*
     * Tiles are as follows:
     * 0 - passable
     * 1 - impassable
     * - KTZ
     */

    public GameObject PassableTile;
    public GameObject WallTile;
    public GameObject LeftTile;
    public bool passable;
    public int[][] tileMapTest = new int[][]{ new int[]{1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                                              new int[]{1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                              new int[]{1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                              new int[]{1, 0, 2, 0, 0, 0, 0, 0, 0, 1},
                                              new int[]{1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                              new int[]{1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                              new int[]{1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                              new int[]{1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                              new int[]{1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                              new int[]{1, 1, 1, 1, 1, 1, 1, 1, 1, 1} };

    public void GenerateTileMap()
    { 
        // Instantiate the tile types onto the game world, or placing them - KTZ
        for(int i = 0; i < 10; i++)
            for(int j = 0; j < 10; j++)
            {
                Vector2 tilePos = new Vector2(0.0f + i, 0.0f + j);
                Quaternion tileRot = Quaternion.identity;
                if (tileMapTest[i][j] == 1)
                {
                    Instantiate(WallTile, tilePos, tileRot);
                }
                else if(tileMapTest[i][j] == 2)
                {
                    Instantiate(LeftTile, tilePos, tileRot);
                }
                else
                {
                    Instantiate(PassableTile, tilePos, tileRot);
                }
            }
    }

    public void GenerateCollision()
    { 
        // Put collision on any tiles that are impassible - KTZ
    }


	// Use this for initialization
	void Start () {
        GenerateTileMap();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

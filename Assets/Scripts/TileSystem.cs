using UnityEngine;
using System.Collections;

public class TileSystem : MonoBehaviour {

    /*
     * Tiles are as follows:
     * 0 - passable
     * 1 - wall (impassable)
     * 2 - door (impassable -> passable)
     * - KTZ
     */

    public GameObject PassableTile;
    public GameObject WallTile;
    public GameObject DoorTile;
    public bool passable;
    public int[][] tileMapTest = new int[][]{ new int[]{1, 1, 1, 1, 2, 1, 1, 1, 1, 1},
                                              new int[]{1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                              new int[]{1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                              new int[]{1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
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
                Vector3 tilePos = new Vector3(0.0f + i, 0.0f + j, 0.0f);
                Quaternion tileRot = Quaternion.identity;
                if (tileMapTest[i][j] == 1)
                {
                    GameObject NewTile = Instantiate(WallTile, tilePos, tileRot) as GameObject;
                    NewTile.transform.parent = gameObject.transform;
                }
                else if(tileMapTest[i][j] == 2)
                {
                    GameObject NewTile = Instantiate(DoorTile, tilePos + new Vector3(0.0f, 0.0f, 0.25f), tileRot) 
                        as GameObject;
                    NewTile.transform.parent = gameObject.transform;
                }
                else
                {
                    GameObject NewTile = Instantiate(PassableTile, tilePos + new Vector3(0.0f, 0.0f, 0.25f), tileRot) 
                        as GameObject;
                    NewTile.transform.parent = gameObject.transform;
                }
            }
        transform.Rotate(35.0f, 315.0f, 345.0f);
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

using UnityEngine;
using System.Collections;

public class TileSystem : MonoBehaviour {

    /*
     * Tiles are as follows:
     * 0 - passable
     * 1 - impassable
     * - KTZ
     */
	const int kPassable = 0;
	const int kImpassable = 1;

	static readonly int kNavGridWidth = 10;
	static readonly int kNavGridHeight = 10;

    public GameObject PassableTile;
    public GameObject WallTile;
    public bool passable;
	public int[][] tileMapTest = new int[][]{ new int[]{1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
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
		float xDisplayOffset = (float)(kNavGridWidth) / 2;
		float yDisplayOffset = (float)(kNavGridHeight) / 2;
		for (int i = 0; i < kNavGridWidth; i++) {
			
			for (int j = 0; j < kNavGridHeight; j++) {

				Vector2 tilePos = new Vector2 (i - xDisplayOffset, j - yDisplayOffset);
				Quaternion tileRot = Quaternion.identity;

				switch (tileMapTest [i] [j]) {
				case kPassable:
					Instantiate (PassableTile, tilePos, tileRot);
					break;

				case kImpassable:
					Instantiate (WallTile, tilePos, tileRot);
					break;

				default:
					break;
				}
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

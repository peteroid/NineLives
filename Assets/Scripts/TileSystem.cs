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
    enum TerrainType
    {
        kPass = 0,
        kWall = 1,
        kDoor = 2
    }

	static readonly int kNavGridWidth = 10;
	static readonly int kNavGridHeight = 10;

    public GameObject PassableTile;
    public GameObject WallTile;
    public GameObject DoorTile;
    public bool passable;

    public uint[][] tileMapTest = new uint[][]{ new uint[]{1, 1, 1, 1, 2, 1, 1, 1, 1, 1},
                                                new uint[]{1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                                new uint[]{1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                                new uint[]{1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                                new uint[]{1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                                new uint[]{1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                                new uint[]{1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                                new uint[]{1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                                new uint[]{1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                                new uint[]{1, 1, 1, 1, 1, 1, 1, 1, 1, 1} };

    public void GenerateTileMap()
    {
        float xDisplayOffset = (float)(kNavGridWidth) / 2;
        float yDisplayOffset = (float)(kNavGridHeight) / 2;
        // Instantiate the tile types onto the game world, or placing them - KTZ
        for (uint i = 0; i < kNavGridWidth; i++)
        {
            for (uint j = 0; j < kNavGridHeight; j++)
            {
                Vector3 tilePos = new Vector3(i - xDisplayOffset, j - yDisplayOffset, 0.0f);
                Quaternion tileRot = Quaternion.identity;

                GameObject tileToMake = null;

                switch ((TerrainType)tileMapTest[i][j])
                {
                    case TerrainType.kPass:
                        tileToMake = PassableTile;
                        break;

                    case TerrainType.kWall:
                        tileToMake = WallTile;
                        break;

                    case TerrainType.kDoor:
                        tileToMake = DoorTile;
                        tilePos.z += 0.25f;
                        break;

                    default: break;
                }
                GameObject newTile = (GameObject)Instantiate(tileToMake, tilePos, tileRot);
                newTile.transform.parent = gameObject.transform;
            }
        }
        //transform.Rotate(35.0f, 315.0f, 345.0f);
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

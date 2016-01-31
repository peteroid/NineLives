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
    public enum TerrainType
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

    public Vector3 mDisplayOffset = new Vector3(0.0f, 0.0f, 0.0f);

    public class Tile
    {
        public TerrainType mType;
        public int mX;
        public int mY;
        public bool mPassable;
        
        // Display Information
        public Vector3 mDisplayOffsets = new Vector3(0.0f, 0.0f, 0.0f);
        public GameObject mTileBaseObject;

        public Tile(TileSystem parent, TerrainType type, int x, int y)
        {
            mType = type;
            mX = x;
            mY = y;
            mPassable = true;

            switch (mType)
            {
                case TerrainType.kPass:
                    mTileBaseObject = parent.PassableTile;
                    break;

                case TerrainType.kWall:
                    mPassable = false;
                    mTileBaseObject = parent.WallTile;
                    break;

                case TerrainType.kDoor:
                    mTileBaseObject = parent.DoorTile;
                    mDisplayOffsets.z += 0.25f;
                    break;

                default: break;
            }
        }
    }

    public int[][] tileMapTest = new int[][]{ new int[]{1, 1, 1, 1, 2, 1, 1, 1, 1, 1},
                                              new int[]{1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                              new int[]{1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                              new int[]{1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                              new int[]{1, 0, 0, 1, 0, 1, 0, 0, 0, 1},
                                              new int[]{1, 0, 0, 1, 0, 0, 0, 0, 0, 1},
                                              new int[]{1, 0, 0, 1, 1, 1, 0, 0, 0, 1},
                                              new int[]{1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                              new int[]{1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                                              new int[]{1, 1, 1, 1, 1, 1, 1, 1, 1, 1} };

    public Tile[][] navGrid;

    public void GenerateTileMap()
    {
        // Instantiate the tile types onto the game world, or placing them - KTZ
        for (int x = 0; x < kNavGridWidth; ++x)
        {
            for (int y = 0; y < kNavGridHeight; ++y)
            {
                Vector3 tilePos = new Vector3(y, x, 0.0f);
                tilePos += mDisplayOffset;
                Quaternion tileRot = Quaternion.identity;
                tilePos += navGrid[x][y].mDisplayOffsets;
                tilePos.y *= -1;
                tilePos.y--;
                GameObject newTile = (GameObject)Instantiate(navGrid[x][y].mTileBaseObject, tilePos, tileRot);
                newTile.transform.parent = gameObject.transform;
            }
        }
        //transform.Rotate(35.0f, 315.0f, 345.0f);
	}

    public bool TryMove(int originX, int originY, int dirX, int dirY)
    {
        Debug.Log(string.Format("{0} {1} {2} {3}", originX, originY, dirX, dirY));
        if( dirX >= kNavGridWidth || dirX < 0 ||
            dirY >= kNavGridHeight || dirY < 0)
        {
            return false;
        }

        if(navGrid[dirX][dirY].mPassable == true)
        {
            return true;
        }

        return false;
    }
    

    public void GenerateCollision()
    { 
        // Put collision on any tiles that are impassible - KTZ
    }


	// Use this for initialization
	void Start ()
    {
        mDisplayOffset.x = (float)(-kNavGridWidth) / 2;
        mDisplayOffset.y = (float)(-kNavGridHeight) / 2;

        navGrid = new Tile[kNavGridWidth][];
        for (int x = 0; x < kNavGridWidth; ++x)
        {
            navGrid[x] = new Tile[kNavGridHeight];
            for (int y = 0; y < kNavGridHeight; ++y)
            {
                navGrid[x][y] = new Tile(this, (TerrainType)tileMapTest[x][y], x, y);
            }
        }
        GenerateTileMap();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

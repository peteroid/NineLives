using UnityEngine;
using SimpleJSON;
using System.Collections;
using System.IO;

public class TileSystem : MonoBehaviour {
    
    public enum TerrainType
    {
        kPass = 0,
        kWall = 1,
        kDoor = 2
    }

	private int mWidth = 0;
	private int mHeight = 0;

    public GameObject PassableTile;
    public GameObject WallTile;
    public GameObject DoorTile;

    public Vector3 mDisplayOffset = new Vector3(0.0f, 0.0f, 0.0f);

    public int mPlayerStartX;
    public int mPlayerStartY;

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

    public Tile[][] navGrid;

    public void GenerateTileMap()
    {
        // Instantiate the tile types onto the game world, or placing them - KTZ
        for (int x = 0; x < mWidth; ++x)
        {
            for (int y = 0; y < mHeight; ++y)
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

    public bool TryMove(ITilePlaceable objectToMove, int dirX, int dirY)
    {
        int destX = objectToMove.GetX() + dirX;
        int destY = objectToMove.GetY() + dirY;

        if(destX >= mWidth || destX < 0 ||
            destY >= mHeight || destY < 0)
        {
            return false;
        }

        if(navGrid[destX][destY].mPassable == true)
        {
            objectToMove.SetX(destX);
            objectToMove.SetY(destY);

            return true;
        }

        return false;
    }

    public void LoadMap(string lvlNum)
    {
        TextAsset levelFile = Resources.Load<TextAsset>("Levels/" + lvlNum);
        JSONNode jsonObj = JSON.Parse(levelFile.text);
        
        mWidth = jsonObj["data"].AsArray.Count;
        mHeight = jsonObj["data"][0].AsArray.Count;
        
        mDisplayOffset.x = (float)(-mWidth) / 2;
        mDisplayOffset.y = (float)(-mHeight) / 2;

        navGrid = new Tile[mWidth][];
        for (int x = 0; x < mWidth; ++x)
        {
            navGrid[x] = new Tile[mHeight];
            for (int y = 0; y < mHeight; ++y)
            {
                int tileCode = jsonObj["data"][x][y].AsInt;
                if(tileCode == -1)
                {
                    // Extract as player data, then set the tile type to default
                    mPlayerStartX = x;
                    mPlayerStartY = y;
                    tileCode = 0;
                }
                navGrid[x][y] = new Tile(this, (TerrainType)tileCode, x, y);
            }
        }
    }

	// Use this for initialization
	void Start ()
    {
        LoadMap("1");
        GenerateTileMap();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

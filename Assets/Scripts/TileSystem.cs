using UnityEngine;
using SimpleJSON;
using System.Collections;
using System.IO;
using System;

public class TileSystem : MonoBehaviour {
    
	public int mWidth = 0;
	public int mHeight = 0;

    public GameObject PassableTile;
    public GameObject WallTile;
    public GameObject DoorTile;

    public GameObject SimpleBlock;

    public Vector3 mDisplayOffset = new Vector3(0.0f, 0.0f, 0.0f);

    public int mPlayerStartX;
    public int mPlayerStartY;
    
    public Tile[][] mNavGrid;

    public Tile GetTile(int x, int y)
    {
        if (x >= mWidth || x < 0 ||
            y >= mHeight || y < 0)
        {
            return null;
        }

        return mNavGrid[x][y];
    }

    public bool CanMove(ITilePlaceable obj, int dirX, int dirY)
    {
        Tile dest = GetTile(obj.GetX() + dirX, obj.GetY() + dirY);
        if (dest != null && dest.AllowIncomingMove(obj, dirX, dirY))
        {
            return true;
        }

        return false;
    }

    public void TryMove(ITilePlaceable obj, int dirX, int dirY)
    {
        Tile dest = GetTile(obj.GetX() + dirX, obj.GetY() + dirY);
        if(dest != null)
        {
            dest.TryIncomingMove(obj, dirX, dirY);
        }
    }

    public void LoadMap(string lvlNum)
    {
        TextAsset levelFile = Resources.Load<TextAsset>("Levels/" + lvlNum);
        JSONNode jsonObj = JSON.Parse(levelFile.text);
        
        mWidth = jsonObj["data"].AsArray.Count;
        mHeight = jsonObj["data"][0].AsArray.Count;
        
        mDisplayOffset.x = (float)(-mWidth) / 2;
        mDisplayOffset.y = (float)(-mHeight) / 2;

        mNavGrid = new Tile[mWidth][];
        for (int x = 0; x < mWidth; ++x)
        {
            mNavGrid[x] = new Tile[mHeight];
            for (int y = 0; y < mHeight; ++y)
            {
                int tileCode = jsonObj["data"][x][y].AsInt;
                if(tileCode < 0)
                {
                    mNavGrid[x][y] = new Tile(this, Tile.TerrainType.kPass, x, y);
                    LoadSpecializedItem(mNavGrid[x][y], tileCode);
                }
                else
                {
                    mNavGrid[x][y] = new Tile(this, (Tile.TerrainType)tileCode, x, y);
                }
            }
        }
    }

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
                tilePos += mNavGrid[x][y].mDisplayOffsets;
                tilePos.y *= -1;
                tilePos.y--;
                GameObject newTile = (GameObject)Instantiate(mNavGrid[x][y].mTileBaseObject, tilePos, tileRot);
                newTile.transform.parent = gameObject.transform;

                mNavGrid[x][y].SetTileGameObject(newTile);

                foreach(Block block in mNavGrid[x][y].mPlaceables)
                {
                    if (block != null)
                    {
                        GameObject blockObj = (GameObject)Instantiate(block.mBlockBaseObject, Vector3.zero, Quaternion.identity);
                        block.SetBlockGameObject(blockObj);
                    }
                }
            }
        }
    }

    private void LoadSpecializedItem(Tile tile, int tileCode)
    {
        switch(tileCode)
        {
            case -1: // Simple block
                new Block((Block.BlockType)(tileCode * -1), tile);
                break;

            case -9: // Player position
                mPlayerStartX = tile.mX;
                mPlayerStartY = tile.mY;
                break;

            default: break;
        }
    }

    // Use this for initialization
    void Start ()
    {
        LoadMap("3");
        GenerateTileMap();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

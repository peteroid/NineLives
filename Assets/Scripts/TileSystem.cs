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
    public GameObject RollingBlock;

    public Vector3 mDisplayOffset = new Vector3(0.0f, 0.0f, 0.0f);

    public int mPlayerStartX;
    public int mPlayerStartY;
    
    public Tile[][] mNavGrid;

    private ArrayList mBlocksOnMoveLoop = new ArrayList();

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
        if(tileCode == -9)
        {
            // Player position
            mPlayerStartX = tile.mX;
            mPlayerStartY = tile.mY;
        }
        else
        {
            new Block((Block.BlockType)(tileCode * -1), tile);
        }
    }

    const float kDelays = 0.15f;

    private class BlockUpdate
    {
        public Block block;
        public int dirX;
        public int dirY;
        public float delayUntilNextMove;
    }

    public void AddBlockToUpdateList(Block block, int dirX, int dirY)
    {
        BlockUpdate update = new BlockUpdate();
        update.block = block;
        update.dirX = dirX;
        update.dirY = dirY;
        update.delayUntilNextMove = kDelays;
        mBlocksOnMoveLoop.Add(update);
    }

    // Use this for initialization
    void Start ()
    {
        LoadMap("1");
        GenerateTileMap();
	}


    private float mLastUpdate = 0.0f;
    // Update is called once per frame
    void Update () {
        float now = Time.unscaledTime;
        float delta = now - mLastUpdate;
        mLastUpdate = now;

        ArrayList removeUpdates = new ArrayList();

        foreach(BlockUpdate update in mBlocksOnMoveLoop)
        {
            update.delayUntilNextMove -= delta;
            if(update.delayUntilNextMove < 0.0f)
            {
                update.delayUntilNextMove += kDelays;
                if(update.block.CanMove(update.dirX, update.dirY))
                {
                    update.block.TryMove(update.dirX, update.dirY);
                }
                else
                {
                    removeUpdates.Add(update);
                }
            }
        }

        foreach(BlockUpdate update in removeUpdates)
        {
            mBlocksOnMoveLoop.Remove(update);
        }
	}
}

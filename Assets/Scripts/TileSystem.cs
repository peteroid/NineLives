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
    public GameObject PressureTile;
    public GameObject OmenDoor;

    public GameObject SimpleBlock;
    public GameObject RollingBlock;
    public GameObject AttachableBlock;

    public Vector3 mDisplayOffset = new Vector3(0.0f, 0.0f, 0.0f);

    public int mPlayerStartX;
    public int mPlayerStartY;
    
    private Tile[][] mNavGrid;

    private ArrayList mPlaceableUpdates = new ArrayList();
	private ArrayList mTiles;
	private string[] mLevels;

	private int mLevelIndex = -1;

	public TileSystem ()
	{
		mTiles = new ArrayList ();
	}

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
			dest.TryIncomingMove (obj, dirX, dirY);
        }
    }

	private void LoadLevels ()
	{
		TextAsset levelFile = Resources.Load<TextAsset>("Levels/levels");
		JSONNode jsonObj = JSON.Parse(levelFile.text);

		int levelCount = jsonObj["data"].AsArray.Count;
		if (levelCount > 0)
		{
			mLevelIndex = 0;
			mLevels = new string[levelCount];
			for (int i = 0; i < levelCount; i++)
			{
				mLevels[i] = jsonObj["data"][i];
				Debug.Log (mLevels[i].ToString ());
			}
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
                    mNavGrid[x][y] = new Tile(this, Tile.TerrainType.kPass, x, y, 0);
                    LoadSpecializedItem(mNavGrid[x][y], tileCode);
                }
                else
                {
                    int id = 0;
                    if(tileCode >= 10)
                    {
                        id = tileCode % 10;
                        tileCode -= id;
                    }

                    mNavGrid[x][y] = new Tile(this, (Tile.TerrainType)tileCode, x, y, id);
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
				mTiles.Add (newTile);

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

    private class PlaceableUpdate
    {
        public ITilePlaceable placeable;
        public int dirX;
        public int dirY;
        public float delayUntilNextMove;
    }

    public void AddToUpdateList(ITilePlaceable placeable, int dirX, int dirY, int duration)
    {
        // Don't duplicate
        if(placeable.GetProperties().inUpdateSequenceFor > 0)
        {
            ArrayList removeUpdates = new ArrayList();
            foreach (PlaceableUpdate rUpdate in mPlaceableUpdates)
            {
                if(rUpdate.placeable == placeable)
                {
                    removeUpdates.Add(rUpdate);
                }
            }
            foreach (PlaceableUpdate rUpdate in removeUpdates)
            {
                mPlaceableUpdates.Remove(rUpdate);
            }
        }

        PlaceableUpdate update = new PlaceableUpdate();
        update.placeable = placeable;
        update.dirX = dirX;
        update.dirY = dirY;
        update.delayUntilNextMove = kDelays;

        placeable.GetProperties().inUpdateSequenceFor = duration;

        mPlaceableUpdates.Add(update);
    }

    // Use this for initialization
    void Start ()
    {
		LoadLevels ();
		LoadCurrentLevel ();
	}

	public void LoadCurrentLevel ()
	{
		LoadMap(mLevels[mLevelIndex]);
		// clean up the current tiles before generating
		foreach (GameObject tile in mTiles)
		{
			Destroy (tile);
		}

		GenerateTileMap();	
	}

	public void NextLevel ()
	{
		++mLevelIndex;
		LoadCurrentLevel ();
	}


    private float mLastUpdate = 0.0f;
    // Update is called once per frame
    void Update () {
        float now = Time.unscaledTime;
        float delta = now - mLastUpdate;
        mLastUpdate = now;

        ArrayList updates = (ArrayList)mPlaceableUpdates.Clone();

        foreach(PlaceableUpdate update in updates)
        {
            update.delayUntilNextMove -= delta;
            if(update.delayUntilNextMove < 0.0f)
            {
                update.delayUntilNextMove += kDelays;

                PlaceableProperties props = update.placeable.GetProperties();
                bool remove = false;

                if (update.placeable.CanMove(update.dirX, update.dirY))
                {
                    update.placeable.TryMove(update.dirX, update.dirY);
                    props.inUpdateSequenceFor--;
                    if(props.inUpdateSequenceFor <= 0)
                    {
                        remove = true;
                    }
                }
                else
                {
                    remove = true;
                }

                if(remove)
                {
                    update.placeable.GetProperties().inUpdateSequenceFor = 0;
                    mPlaceableUpdates.Remove(update);
                }
            }
        }
	}
}

using UnityEngine;
using SimpleJSON;
using System.IO;
using System.Collections;
using System;

public class TileSystem : MonoBehaviour {
    
	public int mWidth = 0;
	public int mHeight = 0;

    public GameObject PassableTile;
    public GameObject WallTile;
    public GameObject DoorTile;
    public GameObject HumanDoorTile;
    public GameObject EmptyTile;
    public GameObject BlockOnlyTile;
    public GameObject PressureTile;
    public GameObject OmenDoor;
    public GameObject PushUpTile;
    public GameObject PushLeftTile;
    public GameObject PushRightTile;
    public GameObject PushDownTile;

    public GameObject SimpleBlock;
    public GameObject RollingBlock;
    public GameObject AttachableBlock;
    public GameObject CommandBlock;

	public SharedDataScript sharedDdataObject;

    public Vector3 mDisplayOffset = new Vector3(0.0f, 0.0f, 0.0f);

    public int mPlayerStartX;
    public int mPlayerStartY;
    
    public Tile[][] mNavGrid;

	private ArrayList mPlaceableUpdates = new ArrayList();
	private ArrayList mTileGameObjs, mPlaceableGameObjs;
	private string[] mLevels;

	public int mLevelIndex = -1;
	private float speedFactor = 1f;

	public TileSystem ()
	{
		mTileGameObjs = new ArrayList ();
		mPlaceableGameObjs = new ArrayList ();
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
			mLevelIndex = (mLevelIndex < 0 ? 0 : mLevelIndex);
			mLevels = new string[levelCount];
			for (int i = 0; i < levelCount; i++)
			{
				mLevels[i] = jsonObj["data"][i];
//				Debug.Log (mLevels[i].ToString ());
			}
		}
	}

    public void LoadMap(string lvlNum)
    {
        if(lvlNum == "8")
        {
            // First level is guaranteed cat
            if(sharedDdataObject.trivialEndings > 1)
            {
                lvlNum = "8cat";
            }
            else
            {
                lvlNum = "8human";
            }
        }
        TextAsset levelFile = Resources.Load<TextAsset>("Levels/" + lvlNum);
        JSONNode jsonObj = JSON.Parse(levelFile.text);
        
		// record the next dialogue scene for the current map
		sharedDdataObject.nextDialogue = jsonObj["dialogue"];

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
                    if(tileCode >= 40)
                    {
                        id = tileCode % 10;
                        tileCode -= id;
                    }

                    mNavGrid[x][y] = new Tile(this, (Tile.TerrainType)tileCode, x, y, id);
                }
            }
        }

        int numExtra = jsonObj["extraSpawns"].AsArray.Count;
        for(int extra = 0; extra < numExtra; ++extra)
        {
            JSONNode node = jsonObj["extraSpawns"][extra];
            int x = node["x"].AsInt;
            int y = node["y"].AsInt;
            LoadSpecializedItem(mNavGrid[x][y], node["id"].AsInt);
        }

		Debug.Log(String.Format("LoadMap: {0}, Trivial Endings: {1}, Dialog: {2}",
			lvlNum, sharedDdataObject.trivialEndings, sharedDdataObject.nextDialogue));
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

				// randomize the position of the tiles first
				Vector3 startFrom = GetRandomVector3 (15f, 30f);
				GameObject newTile = (GameObject)Instantiate(mNavGrid[x][y].mTileBaseObject, startFrom, tileRot);
				newTile.GetComponent<SlideBlock> ().SetStartPosition (tilePos, startFrom.magnitude / 1.2f * speedFactor);

                newTile.transform.parent = gameObject.transform;
				mTileGameObjs.Add (newTile);
                mNavGrid[x][y].SetTileGameObject(newTile);
            }
        }
    }

	public void PostGenerateTileMap ()
	{
		for (int x = 0; x < mWidth; ++x)
		{
			for (int y = 0; y < mHeight; ++y)
			{
                ArrayList placeables = (ArrayList)mNavGrid[x][y].mPlaceables.Clone();
				foreach(System.Object block in placeables)
				{
					Block tryBlock = block as Block;
					if (tryBlock != null)
					{
						GameObject blockObj = (GameObject)Instantiate(tryBlock.mBlockBaseObject, Vector3.zero, Quaternion.identity);
						tryBlock.SetBlockGameObject(blockObj);
						mPlaceableGameObjs.Add (blockObj);
						Debug.Log ("block generated");
					}
				}
			}
		}
	}

	public void SetSpeedFactor (float speedFactor)
	{
		this.speedFactor = speedFactor;
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

    public void RemoveFromUpdateList(ITilePlaceable placeable)
    {
        if (placeable.GetProperties().inUpdateSequenceFor > 0)
        {
            ArrayList removeUpdates = new ArrayList();
            foreach (PlaceableUpdate rUpdate in mPlaceableUpdates)
            {
                if (rUpdate.placeable == placeable)
                {
                    removeUpdates.Add(rUpdate);
                }
            }
            foreach (PlaceableUpdate rUpdate in removeUpdates)
            {
                mPlaceableUpdates.Remove(rUpdate);
            }
        }
    }

    public void Delete(GameObject obj)
    {
        Destroy(obj);
    }

    public void AddToUpdateList(ITilePlaceable placeable, int dirX, int dirY, int duration)
    {
        // Don't duplicate
        RemoveFromUpdateList(placeable);

        PlaceableUpdate update = new PlaceableUpdate();
        update.placeable = placeable;
        update.dirX = dirX;
        update.dirY = dirY;
        update.delayUntilNextMove = kDelays;

        update.placeable.GetProperties().inUpdateSequenceFor = duration;

        mPlaceableUpdates.Add(update);
    }

    // Use this for initialization
    void Start ()
    {
		LoadLevels ();
//		mLevelIndex = (sharedDdataObject.levelIndex > mLevelIndex? sharedDdataObject.levelIndex : mLevelIndex);
		mLevelIndex = sharedDdataObject.levelIndex;
		Debug.Log (sharedDdataObject.levelIndex);
		LoadCurrentLevel ();
	}

	public void LoadCurrentLevel ()
    {
        mPlaceableUpdates.Clear();
        foreach (GameObject placeable in mPlaceableGameObjs)
		{
			Delete (placeable);
		}
		mPlaceableGameObjs.Clear ();

		foreach (GameObject tile in mTileGameObjs)
		{
			Delete (tile);
		}
		mTileGameObjs.Clear ();

        // clean up the current tiles before generating
        DelegateHost.Recreate();
        LoadMap(mLevels[mLevelIndex]);
        GenerateTileMap ();
    }

	public void PreNextLevel ()
	{
		sharedDdataObject.levelIndex = ++mLevelIndex;
		Debug.Log (mLevelIndex);
		foreach (GameObject placeable in mPlaceableGameObjs)
		{
			Delete (placeable);
		}
		mPlaceableGameObjs.Clear ();

		UnityEngine.Random.seed = (int) Time.time;
//		Debug.Log (UnityEngine.Random.seed.ToString ());
		foreach (GameObject tile in mTileGameObjs)
		{
			Vector3 velocity = GetRandomVector3 (7f * speedFactor * 1.5f, 10f * speedFactor * 1.5f);
			tile.GetComponent<SlideBlock> ().SetVelocity (velocity);
		}
	}

	public Vector3 GetRandomVector3 (float min, float max)
	{
		float axisDeterminator = UnityEngine.Random.value;
		float randomizedSpeed = UnityEngine.Random.Range (min, max) * (UnityEngine.Random.value > 0.5f ? 1 : -1);
		if (axisDeterminator < 1.0f / 3) {
			return new Vector3 (randomizedSpeed, 0, 0);
		} else if (axisDeterminator > 1.0f / 3 * 2) {
			return new Vector3 (0, randomizedSpeed, 0);
		} else {
			return new Vector3 (0, 0, randomizedSpeed);
		}
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
                bool move = false;

                if (update.placeable.CanMove(update.dirX, update.dirY))
                {
                    move = true;
                    props.inUpdateSequenceFor--;
                    if (props.inUpdateSequenceFor <= 0)
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
                    update.placeable.GetProperties().inUpdateSequenceFor = -1;
                    mPlaceableUpdates.Remove(update);
                }

                if(move)
                {
                    update.placeable.TryMove(update.dirX, update.dirY);
                }
            }
        }
	}
}

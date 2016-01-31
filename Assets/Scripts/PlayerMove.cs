using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class PlayerMove : MonoBehaviour, InputInterface, ITilePlaceable {

	public enum WinType
	{
		trivial,
		humanize
	}

	// drag and drop the input to this GameObject
	public InputScript input;
    public TileSystem navGrid;
	public Transform spriteTransform;
    public GUIText textObject;

    private int mX;
    private int mY;

    private bool mInitialized = false;

    private ITile mOwningTile = null;

    private int mMeowCount = 0;
    private int mHumanCount = 0;

    private GUITexture mFlash;

    private void Move(int x, int y)
    {
        if (navGrid.CanMove(this, x, y))
        {
            navGrid.TryMove(this, x, y);

            bool moveToNext = true;
			// check for win conditions
			switch (((Tile) mOwningTile).mType)
			{
				case Tile.TerrainType.kDoor:
                    mMeowCount++;
                    break;
				case Tile.TerrainType.kHumanDoor:
                    mHumanCount++;
					break;
				default:
                    moveToNext = false;
                    break;
			}
            if(moveToNext)
            {
                NextLevel();
            }
        }
    }

	private void NextLevel ()
	{
        mFlash.enabled = true;

        navGrid.NextLevel ();
		Init ();
	}

	public void ResetLevel ()
	{
		Debug.Log ("reset");
		navGrid.LoadCurrentLevel ();
		Init ();
	}

	// the shorthands are messed up due to the rotation of camera
	public void Up () {
        Move(0, 1);
		Face ("right");
	}

	public void Left ()
    {
        Move(-1, 0);
		Face ("left");
	}

	public void Right ()
    {
        Move(1, 0);
		Face ("right");
	}

	public void Down ()
    {
        Move(0, -1);
		Face ("left");
	}

	// -1 for left, 1 for right
	private void Face (string direction)
	{
		Vector3 scale = spriteTransform.localScale;
		spriteTransform.localScale = new Vector3 ((direction == "left"? -1 : 1) * Math.Abs (scale.x), scale.y, scale.z);
	}

	private void Init()
	{
		mX = 0;
		mY = 0;
		mInitialized = false;
	}

	// Use this for initialization
	void Start () {
		Init ();

        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.white);
        tex.Apply();
        GameObject storageGB = new GameObject("Flash");
        storageGB.transform.localScale = new Vector3(0, 0, 1);
        mFlash = storageGB.AddComponent<GUITexture>();
        mFlash.pixelInset = new Rect(0, 0, Screen.width, Screen.height);
        mFlash.color = Color.black;
        mFlash.texture = tex;
        mFlash.enabled = false;

        mProperties.isPlayer = true;
        mProperties.canPushBlocks = true;

        input.SetInputInterface (this);
	}

    void PostStart()
    {
        Move(navGrid.mPlayerStartX, navGrid.mPlayerStartY);
    }
	
	// Update is called once per frame
	void Update () {
	    if(mInitialized == false)
        {
            mInitialized = true;
            PostStart();
        }
	}

    public int GetX()
    {
        return mX;
    }

    public int GetY()
    {
        return mY;
    }

    public void SetX(int x)
    {
        mX = x;
    }

    public void SetY(int y)
    {
        mY = y;
    }

    public bool AllowIncomingMove(ITilePlaceable incomingPlaceable, int dirX, int dirY)
    {
        return false;
    }

    public void TryIncomingMove(ITilePlaceable incomingPlaceable, int dirX, int dirY)
    {
        // Should never occur
    }

    public void SetAsOwningTile(ITile tile)
    {
        if(mOwningTile != null)
        {
            mOwningTile.Unsubscribe(this);
        }
        tile.Subscribe(this);

        mOwningTile = tile;
    }

    public void SetVisualPosition(Vector3 position)
    {
        transform.position = new Vector3(position.x, position.y, transform.position.z);
    }

    private PlaceableProperties mProperties = new PlaceableProperties();
    public PlaceableProperties GetProperties()
    {
        return mProperties;
    }
}

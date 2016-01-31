using UnityEngine;
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

    private int mX;
    private int mY;

    private bool mInitialized = false;

    private ITile mOwningTile = null;

    private void Move(int x, int y)
    {
        if(navGrid.CanMove(this, x, y))
        {
            navGrid.TryMove(this, x, y);

			// check for win conditions
			switch (((Tile) mOwningTile).mType)
			{
				case Tile.TerrainType.kDoor:
					NextLevel ();
					Debug.Log ("Meow");
					break;
				case Tile.TerrainType.kHumanDoor:
					NextLevel ();
					Debug.Log ("Human");
					break;
				default:
					break;
			}
        }
    }

	private void NextLevel ()
	{
		navGrid.NextLevel ();
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

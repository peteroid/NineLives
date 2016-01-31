using UnityEngine;
using System.Collections;
using System;

public class PlayerMove : MonoBehaviour, InputInterface, ITilePlaceable {

	// drag and drop the input to this GameObject
	public GameObject input;
    public TileSystem navGrid;

    private int mX;
    private int mY;

    private bool mInitialized = false;

    private ITile mOwningTile = null;

    private void Move(int x, int y)
    {
        if(navGrid.CanMove(this, x, y))
        {
            navGrid.TryMove(this, x, y);
        }
    }

	// the shorthands are messed up due to the rotation of camera
	public void Up () {
        Move(0, 1);
	}

	public void Left ()
    {
        Move(-1, 0);
	}

	public void Right ()
    {
        Move(1, 0);
	}

	public void Down ()
    {
        Move(0, -1);
	}

	// Use this for initialization
	void Start () {
        mX = 0;
        mY = 0;
        mProperties.isPlayer = true;
        mProperties.canPushBlocks = true;


        input.GetComponent<InputScript> ().SetInputInterface (this);
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

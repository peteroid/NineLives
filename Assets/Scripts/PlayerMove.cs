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

    private void TryMove(int x, int y)
    {
        int oldX = mX;
        int oldY = mY;
        if (navGrid.TryMove(this, x, y))
        {
            
            Vector3 transform = new Vector3(mY - oldY, oldX - mX);
            this.transform.position += transform;
        }
    }

	// the shorthands are messed up due to the rotation of camera
	public void Up () {
        TryMove(0, 1);
	}

	public void Left ()
    {
        TryMove(-1, 0);
	}

	public void Right ()
    {
        TryMove(1, 0);
	}

	public void Down ()
    {
        TryMove(0, -1);
	}

	// Use this for initialization
	void Start () {
        mX = 0;
        mY = 0;

		input.GetComponent<InputScript> ().SetInputInterface (this);
	}

    void PostStart()
    {
        // The nav grid has now loaded
        this.transform.position += navGrid.mDisplayOffset;
        TryMove(navGrid.mPlayerStartX, navGrid.mPlayerStartY);
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
}

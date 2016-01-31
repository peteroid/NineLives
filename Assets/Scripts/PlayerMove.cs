using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class PlayerMove : MonoBehaviour, InputInterface, ITilePlaceable
{

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


    private bool mInitialized = false;

    private int mMeowCount = 0;
    private int mHumanCount = 0;

    private void Move(int dirX, int dirY)
    {
        if(mProperties.inUpdateSequenceFor > 0)
        {
            return;
        }
        if (navGrid.CanMove(this, dirX, dirY))
        {
            MoveFollowers(dirX, dirY);

            navGrid.TryMove(this, dirX, dirY);
            

            bool moveToNext = true;
			// check for win conditions
			switch (((Tile) mOwningTile).mType)
			{
				case Tile.TerrainType.kExit:
                    mMeowCount++;
                    break;
				case Tile.TerrainType.kHumanExit:
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

    public bool CanMove(int dirX, int dirY)
    {
        return navGrid.CanMove(this, dirX, dirY);
    }

    public void TryMove(int dirX, int dirY)
    {
        navGrid.TryMove(this, dirX, dirY);
    }

	private void NextLevel ()
	{
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

    public bool AllowIncomingMove(ITilePlaceable incomingPlaceable, int dirX, int dirY)
    {
        return false;
    }

    public void TryIncomingMove(ITilePlaceable incomingPlaceable, int dirX, int dirY)
    {
        // Should never occur
    }

    public void SetVisualPosition(Vector3 position)
    {
        ParticleSystem p = GetComponent<ParticleSystem>();
        p.Play();
        transform.position = new Vector3(position.x, position.y, transform.position.z);
        //p.Pause();
    }

    //////

    protected int mX = 0;
    protected int mY = 0;
    protected ArrayList mFollowers = new ArrayList();
    protected Tile mOwningTile = null;
    protected PlaceableProperties mProperties = new PlaceableProperties();

    public void Attach(ITilePlaceable follower)
    {
        mFollowers.Add(follower);
    }

    public void Detach(ITilePlaceable follower)
    {
        mFollowers.Remove(follower);
    }

    protected void MoveFollowers(int dirX, int dirY)
    {
        if (mOwningTile == null)
        {
            return;
        }

        ITile siblingTile = mOwningTile.GetSiblingTile(dirX, dirY);
        if (mFollowers.Count > 0 && siblingTile != null)
        {
            ArrayList detachList = new ArrayList();
            foreach (ITilePlaceable obj in mFollowers)
            {
                if (siblingTile.AllowIncomingMove(obj, dirX, dirY))
                {
                    siblingTile.TryIncomingMove(obj, dirX, dirY);
                }
                else
                {
                    detachList.Add(obj);
                }
            }
            foreach (ITilePlaceable obj in detachList)
            {
                Detach(obj);
            }
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

    public void SetAsOwningTile(ITile tile)
    {
        if (mOwningTile != null)
        {
            mOwningTile.Unsubscribe(this);
        }
        tile.Subscribe(this);

        mOwningTile = (Tile)tile;
    }


    public PlaceableProperties GetProperties()
    {
        return mProperties;
    }
}

using UnityEngine;
using System.Collections;
using System;

public class Block : ITilePlaceable {
    public enum BlockType
    {
        kSimple = 1,
        kRolling = 2,
        kAttachable = 3
    }

    private BlockType mType;
    private Vector3 mDisplayOffset = new Vector3();

    public GameObject mBlockBaseObject;
    private GameObject mBlockObject;

    public Block(BlockType type, Tile tile)
    {
        mType = type;
        mX = tile.mX;
        mY = tile.mY;
        SetAsOwningTile(tile);
        switch (mType)
        {
            case BlockType.kSimple:
                mBlockBaseObject = tile.mParentNavGrid.SimpleBlock;
                break;

            case BlockType.kRolling:
                mBlockBaseObject = tile.mParentNavGrid.RollingBlock;
                mProperties.sticksInUpdateFor = 100;
                break;

            case BlockType.kAttachable:
                mBlockBaseObject = tile.mParentNavGrid.AttachableBlock;
                mProperties.attachable = true;
                mProperties.canBeWalkedOver = true;
                mDisplayOffset.z += -0.25f;
                break;

            default: break;
        }
    }

    public void SetBlockGameObject(GameObject blockObject)
    {
        mBlockObject = blockObject;
        mBlockObject.transform.position += mDisplayOffset;
        mOwningTile.LockToPosition(this);
    }

    public bool AllowIncomingMove(ITilePlaceable incomingPlaceable, int dirX, int dirY)
    {
        if (mProperties.canBeWalkedOver)
        {
            return true;
        }

        if (!incomingPlaceable.GetProperties().canPushBlocks)
        {
            return false;
        }

        ITile siblingTile = mOwningTile.GetSiblingTile(dirX, dirY);
        if(siblingTile == null)
        {
            return false;
        }
        return siblingTile.AllowIncomingMove(this, dirX, dirY);
    }

    public void TryIncomingMove(ITilePlaceable incomingPlaceable, int dirX, int dirY)
    {
        if(!mProperties.canBeWalkedOver)
        {
            TryMove(dirX, dirY);

            if (mProperties.sticksInUpdateFor > 0)
            {
                mOwningTile.mParentNavGrid.AddToUpdateList(this, dirX, dirY, mProperties.sticksInUpdateFor);
            }

        }

        if(mProperties.attachable && !mProperties.isAttached)
        {
            incomingPlaceable.Attach(this);
            mProperties.isAttached = true;
        }
    }

    public bool CanMove(int dirX, int dirY)
    {
        ITile siblingTile = mOwningTile.GetSiblingTile(dirX, dirY);
        if (siblingTile == null)
        {
            return false;
        }
        return siblingTile.AllowIncomingMove(this, dirX, dirY);
    }
        
    public void TryMove(int dirX, int dirY)
    {
        MoveFollowers(dirX, dirY);

        ITile siblingTile = mOwningTile.GetSiblingTile(dirX, dirY);
        siblingTile.TryIncomingMove(this, dirX, dirY);
    }

    public void SetVisualPosition(Vector3 position)
    {
        mBlockObject.transform.position = new Vector3(position.x, position.y, mBlockObject.transform.position.z);
    }

    ////////// ITilePlaceable

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

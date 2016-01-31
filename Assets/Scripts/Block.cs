using UnityEngine;
using System.Collections;
using System;

public class Block : ITilePlaceable {
    public enum BlockType
    {
        kSimple = 1,
        kRolling = 2
    }

    private int mX = 0;
    private int mY = 0;
    private Tile mOwningTile = null;
    private BlockType mType;

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
                mProperties.keepsMoving = true;
                break;

            default: break;
        }
    }

    public void SetBlockGameObject(GameObject blockObject)
    {
        mBlockObject = blockObject;
        mOwningTile.LockToPosition(this);
    }

    public bool AllowIncomingMove(ITilePlaceable incomingPlaceable, int dirX, int dirY)
    {
        if(!incomingPlaceable.GetProperties().canPushBlocks)
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
        ITile siblingTile = mOwningTile.GetSiblingTile(dirX, dirY);
        siblingTile.TryIncomingMove(this, dirX, dirY);

        if(mProperties.keepsMoving)
        {
            mOwningTile.mParentNavGrid.AddBlockToUpdateList(this, dirX, dirY);
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
        ITile siblingTile = mOwningTile.GetSiblingTile(dirX, dirY);
        siblingTile.TryIncomingMove(this, dirX, dirY);
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

    public void SetVisualPosition(Vector3 position)
    {
        mBlockObject.transform.position = new Vector3(position.x, position.y, mBlockObject.transform.position.z);
    }

    private PlaceableProperties mProperties = new PlaceableProperties();
    public PlaceableProperties GetProperties()
    {
        return mProperties;
    }
}

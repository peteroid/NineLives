using UnityEngine;
using System.Collections;
using System;

public class Tile : ITile
{
    public enum TerrainType
    {
        kPass = 0,
        kWall = 1,
        kDoor = 2
    }

    public TerrainType mType;
    public int mX;
    public int mY;
    public bool mPassable;

    public ArrayList mPlaceables;

    // Display Information
    public Vector3 mDisplayOffsets = new Vector3(0.0f, 0.0f, 0.0f);
    public GameObject mTileBaseObject;
    public GameObject mTileObject;

    public Tile(TileSystem parent, TerrainType type, int x, int y)
    {
        mType = type;
        mX = x;
        mY = y;
        mPlaceables = new ArrayList();
        mPassable = true;

        switch (mType)
        {
            case TerrainType.kPass:
                mTileBaseObject = parent.PassableTile;
                break;

            case TerrainType.kWall:
                mPassable = false;
                mTileBaseObject = parent.WallTile;
                break;

            case TerrainType.kDoor:
                mTileBaseObject = parent.DoorTile;
                mDisplayOffsets.z += 0.25f;
                break;

            default: break;
        }
    }

    public void SetTileGameObject(GameObject tileGameObject)
    {
        mTileObject = tileGameObject;
    }

    public bool AllowIncomingMove(ITilePlaceable interferingObj, int dirX, int dirY)
    {
        if (!mPassable)
        {
            return false;
        }

        foreach (ITilePlaceable obj in mPlaceables)
        {
            if (!obj.AllowIncomingMove(interferingObj, dirX, dirY))
            {
                return false;
            }
        }

        return true;
    }

    public void TryIncomingMove(ITilePlaceable interferingObj, int dirX, int dirY)
    {
        foreach (ITilePlaceable obj in mPlaceables)
        {
            obj.TryIncomingMove(interferingObj, dirX, dirY);
        }

        interferingObj.SetAsOwningTile(this);
        LockToPosition(interferingObj);
    }

    public void Subscribe(ITilePlaceable placeable)
    {
        mPlaceables.Add(placeable);
    }

    public void Unsubscribe(ITilePlaceable placeable)
    {
        mPlaceables.Remove(placeable);
    }

    public Vector3 GetVisualPosition()
    {
        return mTileObject.transform.position;
    }

    public void LockToPosition(ITilePlaceable placeable)
    {
        placeable.SetX(mX);
        placeable.SetY(mY);
        placeable.SetVisualPosition(mTileObject.transform.position);
    }
}
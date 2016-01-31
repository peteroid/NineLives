using UnityEngine;
using System.Collections;
using System;

public class Tile : ITile
{
    public enum TerrainType
    {
        kPass = 0,
        kWall = 1,
        kExit = 2,
		kHumanExit = 3,
        kBlockHole = 4,

        kPushUp = 10,
        kPushRight = 11,
        kPushDown = 12,
        kPushLeft = 13,

        kPressure = 40,
        
        kPressureDoor = 50
    }

    public TerrainType mType;
    public int mX;
    public int mY;
    public bool mPlayerPassable = true;
    public bool mBlockPassable = true;

    public ArrayList mPlaceables;

    // Display Information
    public Vector3 mDisplayOffsets = new Vector3(0.0f, 0.0f, 0.0f);
    public GameObject mTileBaseObject;
    public GameObject mTileObject;

    public TileSystem mParentNavGrid;

    private bool mIsDoor = false;

    private int mPushDirX = 0;
    private int mPushDirY = 0;

    private int mSpecialCaseID = 0;

    public Tile(TileSystem parent, TerrainType type, int x, int y, int id)
    {
        mType = type;
        mX = x;
        mY = y;
        mPlaceables = new ArrayList();
        mParentNavGrid = parent;
        mSpecialCaseID = id;

        switch (mType)
        {
            case TerrainType.kPass:
                mTileBaseObject = parent.PassableTile;
                mDisplayOffsets.z -= 0.1f;
                break;

            case TerrainType.kWall:
                mPlayerPassable = false;
                mBlockPassable = false;
                mTileBaseObject = parent.WallTile;
                mDisplayOffsets.z -= 0.15f;
                break;

            case TerrainType.kExit:
                mTileBaseObject = parent.DoorTile;
                mDisplayOffsets.z += 0.15f;
                mBlockPassable = false;
                break;

			case TerrainType.kHumanExit:
				mTileBaseObject = parent.DoorTile;
				mDisplayOffsets.z += 0.15f;
                mBlockPassable = false;
                break;

            case TerrainType.kBlockHole:
                mTileBaseObject = parent.EmptyTile;
                mPlayerPassable = false;
                break;

            case TerrainType.kPushUp:
                mTileBaseObject = parent.PushUpTile;
                mDisplayOffsets.z -= 0.1f;
                mPushDirY = 1;
                break;

            case TerrainType.kPushRight:
                mTileBaseObject = parent.PushRightTile;
                mDisplayOffsets.z -= 0.1f;
                mPushDirX = 1;
                break;

            case TerrainType.kPushDown:
                mTileBaseObject = parent.PushDownTile;
                mDisplayOffsets.z -= 0.1f;
                mPushDirY = -1;
                break;

            case TerrainType.kPushLeft:
                mTileBaseObject = parent.PushLeftTile;
                mDisplayOffsets.z -= 0.1f;
                mPushDirX = -1;
                break;

            case TerrainType.kPressure:
                mTileBaseObject = parent.PressureTile;
                mDisplayOffsets.z -= 0.2f;
                break;

            case TerrainType.kPressureDoor:
                mTileBaseObject = parent.OmenDoor;
                mDisplayOffsets.z -= 0.15f;
                mIsDoor = true;
                mPlayerPassable = false;
                mBlockPassable = false;
                DelegateHost.OnPressureChange += HandleOnPressureChange;
                break;

            default: break;
        }
    }

    ~Tile()
    {
        switch(mType)
        {
            case TerrainType.kPressureDoor:
                DelegateHost.OnPressureChange -= HandleOnPressureChange;
                break;

            default: break;
        }
    }

    private void HandleOnPressureChange(bool state, int id)
    {
        if(id == mSpecialCaseID)
        {
            mPlayerPassable = state;
            mBlockPassable = state;
            mTileObject.transform.position += new Vector3(0.0f, 0.0f, (state ? .35f : -.35f));
        }
    }

    public void SetTileGameObject(GameObject tileGameObject)
    {
        mTileObject = tileGameObject;

        if(mType == TerrainType.kPushUp || mType == TerrainType.kPushDown)
        {
            mTileObject.transform.Rotate(0.0f, 0.0f, 90.0f);
        }
    }

    public ITile GetSiblingTile(int dirX, int dirY)
    {
        return mParentNavGrid.GetTile(mX + dirX, mY + dirY);
    }

    public bool AllowIncomingMove(ITilePlaceable interferingObj, int dirX, int dirY)
    {
        if (!mPlayerPassable && interferingObj.GetProperties().isPlayer)
        {
            return false;
        }
        
        if(!mBlockPassable && !interferingObj.GetProperties().isPlayer)
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
        ArrayList moveList = new ArrayList();
        foreach (ITilePlaceable obj in mPlaceables)
        {
            if(obj.GetProperties().canBeWalkedOver)
            {
                obj.TryIncomingMove(interferingObj, dirX, dirY);
            }
            else
            {
                moveList.Add(obj);
            }
        }

        if(moveList.Count > 0)
        {
            foreach (ITilePlaceable obj in moveList)
            {
                obj.TryIncomingMove(interferingObj, dirX, dirY);
            }

                   
            if(interferingObj.GetProperties().isPlayer)
            {
                return;
            }
        }

        if(mType == TerrainType.kBlockHole && !interferingObj.GetProperties().isPlayer)
        {
            mParentNavGrid.RemoveFromUpdateList(interferingObj);
            interferingObj.PrepForRemoval();
            mParentNavGrid.Delete(interferingObj.GetConnectedObject());
        }
        else
        {
            interferingObj.SetAsOwningTile(this);
            LockToPosition(interferingObj);

            if (mPushDirX != 0 || mPushDirY != 0)
            {
                int forcedPushAmount = Math.Max(interferingObj.GetProperties().sticksInUpdateFor, 1);
                mParentNavGrid.AddToUpdateList(interferingObj, mPushDirX, mPushDirY, forcedPushAmount);
            }
        }
    }

    public void Subscribe(ITilePlaceable placeable)
    {
        mPlaceables.Add(placeable);
        if(mType == TerrainType.kPressure && mPlaceables.Count == 1)
        {
            DelegateHost.OnPressureChange.Invoke(true, mSpecialCaseID);
        }
    }

    public void Unsubscribe(ITilePlaceable placeable)
    {
        mPlaceables.Remove(placeable);
        if (mType == TerrainType.kPressure && mPlaceables.Count == 0)
        {
            DelegateHost.OnPressureChange.Invoke(false, mSpecialCaseID);
        }
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
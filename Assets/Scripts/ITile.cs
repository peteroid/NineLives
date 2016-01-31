using UnityEngine;
using System.Collections;

public interface ITile {
    void Unsubscribe(ITilePlaceable placeable);
    void Subscribe(ITilePlaceable placeable);
    void LockToPosition(ITilePlaceable placeable);
    ITile GetSiblingTile(int dirX, int dirY);
    bool AllowIncomingMove(ITilePlaceable interferingObj, int dirX, int dirY);
    bool TryIncomingMove(ITilePlaceable interferingObj, int dirX, int dirY);
}

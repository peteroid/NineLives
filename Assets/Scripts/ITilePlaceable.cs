using UnityEngine;
using System.Collections;

public interface ITilePlaceable
{
    int GetX();
    int GetY();
    void SetX(int x);
    void SetY(int y);

    bool AllowIncomingMove(ITilePlaceable incomingPlaceable, int dirX, int dirY);
    void TryIncomingMove(ITilePlaceable incomingPlaceable, int dirX, int dirY);

    void SetAsOwningTile(ITile tile);
}

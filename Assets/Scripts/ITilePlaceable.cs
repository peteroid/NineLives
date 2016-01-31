using UnityEngine;
using System.Collections;

public interface ITilePlaceable
{
    int GetX();
    int GetY();

    bool AllowIncomingMove(ITilePlaceable incomingPlaceable, int dirX, int dirY);
    void TryIncomingMove(ITilePlaceable incomingPlaceable, int dirX, int dirY);
}

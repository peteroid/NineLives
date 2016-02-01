using UnityEngine;
using System.Collections;

public interface ITile {
    void Unsubscribe(ITilePlaceable placeable);
    void Subscribe(ITilePlaceable placeable);
}

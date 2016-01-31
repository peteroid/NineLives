using UnityEngine;
using System.Collections;

public class PlaceableProperties {
    public bool isPlayer = false;
    public bool canPushBlocks = false;
    public bool keepsMoving = false;
    public bool attachable = false;
    public bool canBeWalkedOver = false;

    public bool isAttached = false;
    public ITilePlaceable attachedTo = null;
}

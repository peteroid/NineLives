using UnityEngine;
using System.Collections;

public class PlaceableProperties {
    public bool isPlayer = false;
    public bool canPushBlocks = false;
    public bool keepsMoving = false;
    public bool attachable = false;
    public ITilePlaceable attachedTo = null;
    public bool isAttached = false;
    public bool canBePushed = true;
}

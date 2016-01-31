using UnityEngine;
using System.Collections;

public class PlaceableProperties {
    public bool isPlayer = false;
    public bool canPushBlocks = false;
    public bool attachable = false;
    public bool canBeWalkedOver = false;
    public int sticksInUpdateFor = 0;

    public int inUpdateSequenceFor = 0;

    public bool isAttached = false;
    public ITilePlaceable attachedTo = null;
}

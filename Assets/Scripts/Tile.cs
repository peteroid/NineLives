using UnityEngine;
using System.Collections;
using System;

 public class Tile 
{
    int tileId;
    int[] toys;
    int triggerId;

    public int getId()
    {
        return tileId;
    }

    public int[] getToys()
    {
        return toys;
    }

    public int getTrigger()
    {
        return triggerId;
    }
  
}
using UnityEngine;
using System.Collections;
using System;

 public class Tile 
{
    int tileId;
    int[] toys;
    int triggerId;




    public Tile(int Id, int[] toysOnTile, int trigger)
    {
        tileId = Id;
        toys = toysOnTile;
        triggerId = trigger;
    }




    public int Id
    {
        get { return tileId; }
        set { tileId = Id; }
    }

    public int[] toyList
    {
        get { return toys; }
        set { toys = toyList; }
    }

    public int trigger
    {
        get { return triggerId; }
        set { triggerId = trigger; }
    } 
  
}
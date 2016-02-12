using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
    protected enum StatusEffect
    {
        //nothing to see here yet but I want it to exist
    };

    public enum CharacterType
    {
        player,
        npc,
        toy
    };

    public Vector2 startingPosition;


    protected CharacterType character { get; set; }
    protected Vector2 currentPosition { get; set; }
    protected int moveSpeed { get; set; }
    protected StatusEffect status { get; set; }

    protected void Move(Vector2 direction, Vector2 source)
    {
        Vector2 target = (source + direction);


        if (Tile.canEnter(target, direction))
        {
            Tile.UpdateToys(target);
            currentPosition = target;
            ScreenCast.UpdateDisplay(this, currentPosition);
        }
        else return;


    }



}

public class Player : Character
{

    public Player()

    {
        character = CharacterType.player;
        currentPosition = startingPosition;
        moveSpeed = 1;
    }    

    
     public void Start()
    {

    }

    public void Update()
    {

    }

  
}

public class NPC : Character
{
    //not used for now
}
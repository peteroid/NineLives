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
        NPC
    };

    public Vector2 startingPosition;


    protected CharacterType character { get; set; }
    protected Vector2 currentPosition { get; set; }
    protected int moveSpeed { get; set; }
    protected StatusEffect status { get; set; }




}

public class Player : Character
{

  public Player()

    {
        character = CharacterType.player;
        currentPosition = startingPosition;
    }    
}

public class NPC : Character
{
    //not used for now
}
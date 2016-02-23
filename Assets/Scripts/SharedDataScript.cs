using UnityEngine;
using System.Collections;

public class SharedDataScript : MonoBehaviour {

	static int _levelIndex;
	public int levelIndex {
		get { return _levelIndex; }
		set { _levelIndex = value; }
	}

	static Tile.TerrainType _type;
	public Tile.TerrainType type {
		get { return _type; }
		set { _type = value; }
	}

    static int _trivialEndings = 0;
    public int trivialEndings
    {
        get { return _trivialEndings; }
        set { _trivialEndings = value; }
    }

	static string _nextDialogue = "";
	public string nextDialogue
	{
		get { return _nextDialogue; }
		set { _nextDialogue = value; }
	}
}

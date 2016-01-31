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

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

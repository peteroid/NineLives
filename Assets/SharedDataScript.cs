using UnityEngine;
using System.Collections;

public class SharedDataScript : MonoBehaviour {

	static int _levelIndex;
	public int levelIndex {
		get { return _levelIndex; }
		set { _levelIndex = value; }
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

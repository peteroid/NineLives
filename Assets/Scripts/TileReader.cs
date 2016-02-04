using UnityEngine;
using System.Collections;

public class TileReader : MonoBehaviour {
    public GameObject[] tileMapSave;
    public GameObject[] toyMapSave;
    // Use this for initialization
    public void GetObjecID(GameObject obj);
        {
            
        }
	void Start ()
    {
       tileMapSave = GameObject.FindGameObjectsWithTag("Tile");
       toyMapSave = GameObject.FindGameObjectsWithTag("Placeable");
       
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

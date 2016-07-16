using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class TileReader : MonoBehaviour {

    public class JTile
    {
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public int ID { get; set; }
        public string type { get; set; }
       

   
    }

    GameObject[] CurrentLevelTiles = GameObject.FindGameObjectsWithTag("Tile");

    GameObject[] CurrentLevelToys = GameObject.FindGameObjectsWithTag("Toy");

    
    public string GenerateJTiles(GameObject[] tiles)
    {

        List<JTile> tileList = new List<JTile>();



        foreach (GameObject tile in tiles)
        {
            JTile currentJTile = new JTile {
                x = tile.transform.position.x,
                y = tile.transform.position.y,
                z = tile.transform.position.z,
                ID = tile.GetComponent<ID>().id,
                type = tile.name
            };

                tileList.Add(currentJTile);
        }



        string jTiles = JsonConvert.SerializeObject(tileList);
           
        return jTiles;
    }



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using SimpleJSON;
using System.IO;

public class DialogueScript : MonoBehaviour {

	public float delay;
	public Tile.TerrainType lastType;
	public Text dialogue1;
	public Text dialogue2;
	public Image spriteCat;
	public Image spriteOther;

	public void LoadGameScene () {
		SceneManager.LoadScene("UnifiedGame");
	}

	// Use this for initialization
	void Start () {
		Invoke ("LoadGameScene", delay);

		TextAsset levelFile = Resources.Load<TextAsset>("Dialogue/StoryData");
//		JSONNode jsonObj = JSON.Parse(levelFile.text);
//
//		JSONArray data = jsonObj["cat"].AsArray;
//		if (lastType == Tile.TerrainType.kHumanExit)
//			data = jsonObj["human"].AsArray;
//
//		JSONArray order = data["order"].AsArray;
//		 
//		for (int i = 0; i < order.Count; i++)
//		{
//			Debug.Log (order[i]);
//		}


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

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
	public Image image1;
	public Image image2;
	public Sprite spriteCat;
	public Sprite spriteOther;
	public SharedDataScript sharedDataObject;

	public void LoadGameScene () {
		SceneManager.LoadScene("UnifiedGame");
	}

	// Use this for initialization
	void Start () {
		Invoke ("LoadGameScene", delay);

		TextAsset levelFile = Resources.Load<TextAsset>("Levels/StoryData");
		JSONNode jsonObj = JSON.Parse(levelFile.text);

		JSONArray data = jsonObj["catStory"].AsArray;
		Debug.Log (sharedDataObject.type);
		if (sharedDataObject.type == Tile.TerrainType.kHumanExit) {
			Debug.Log ("human");
			data = jsonObj ["human"].AsArray;
		}

		JSONArray order = data[sharedDataObject.levelIndex]["order"].AsArray;
		if (order.Count > 0) {
			dialogue1.text = data[sharedDataObject.levelIndex][order[0]];
//			Debug.Log (order [0].Value == "cat");
			if (order [0].Value == "cat")
				image1.overrideSprite = spriteCat;
			else if (order [0].Value == "other")
				image1.overrideSprite = spriteOther;
		}

		if (order.Count > 1) {
			dialogue2.text = data[sharedDataObject.levelIndex][order[1]];
			Debug.Log (order [1].Value);
			if (order [1].Value == "cat")
				image2.overrideSprite = spriteCat;
			else if (order [1].Value == "other")
				image2.overrideSprite = spriteOther;
			image2.gameObject.SetActive (true);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

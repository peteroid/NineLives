using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System.IO;

public class DialogueScript : MonoBehaviour {

	public float delay;
	public Tile.TerrainType lastType;
	public List<Text> dialogues;
	public List<Image> images;
	public List<Sprite> sprites;
	public SharedDataScript sharedDataObject;

	public void LoadGameScene () {
		SceneManager.LoadScene("UnifiedGame");
	}

	// Use this for initialization
	void Start () {
		Debug.Log ("dialog: " + sharedDataObject.nextDialogue);

		TextAsset levelFile = Resources.Load<TextAsset>("Levels/StoryData_new");
		JSONNode jsonObj = JSON.Parse(levelFile.text)[sharedDataObject.nextDialogue];
		if (jsonObj != null) {
			Invoke ("LoadGameScene", delay);
			sharedDataObject.nextDialogue = "";

			JSONArray dialogJsonArray = jsonObj ["storyline"].AsArray;
			for (int i = 0; i < dialogJsonArray.Count; i++) {
				dialogues [i].text = dialogJsonArray [i] ["sentence"];
				images [i].overrideSprite = (dialogJsonArray [i] ["image"].Value == "cat" ?
					sprites [0] : sprites [1]);
				images [i].gameObject.SetActive (true);
//				Debug.Log (images [i].sprite.name);
			}
		} else {
			LoadGameScene ();
		}

//		JSONArray data = jsonObj["catStory"].AsArray;
//		Debug.Log (sharedDataObject.type);
//		if (sharedDataObject.type == Tile.TerrainType.kHumanExit) {
//			Debug.Log ("human");
//			data = jsonObj ["human"].AsArray;
//		}
//
//		JSONArray order = data[sharedDataObject.levelIndex]["order"].AsArray;
//		if (order.Count > 0) {
//			dialogue1.text = data[sharedDataObject.levelIndex][order[0]];
//			if (order [0].Value == "cat")
//				image1.overrideSprite = spriteCat;
//			else if (order [0].Value == "other")
//				image1.overrideSprite = spriteOther;
//		}
//
//		if (order.Count > 1) {
//			dialogue2.text = data[sharedDataObject.levelIndex][order[1]];
//			Debug.Log (order [1].Value);
//			if (order [1].Value == "cat")
//				image2.overrideSprite = spriteCat;
//			else if (order [1].Value == "other")
//				image2.overrideSprite = spriteOther;
//			image2.gameObject.SetActive (true);
//		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

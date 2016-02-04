using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CatEndingScript : MonoBehaviour {

	void LoadScene(){
		SceneManager.LoadScene ("CatEndingScene2");
	}

	// Use this for initialization
	void Start () {
		Invoke ("LoadScene", 7f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}


}

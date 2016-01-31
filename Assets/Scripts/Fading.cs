using UnityEngine;
using System.Collections;

public class Fading : MonoBehaviour {

	public Texture2D tex;
	public float alpha = 1f;
	public float step = 0.2f;
	public float startDelay = 0;
	public float endDelay = 0;
	public int fadeDirection = -1;
	public bool isFading = false;

	public void StartFadeIn () {
		isFading = true;
		fadeDirection = -1;
	}

	public void StartFadeOut () {
		isFading = true;
		fadeDirection = 1;
	}

	void OnGUI() {
		if (isFading) {
			alpha += step * fadeDirection * Time.deltaTime;
			alpha = Mathf.Clamp01 (alpha);
		}

		GUI.depth = -99999;
		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), tex);
	}

	void Start() {
		if (startDelay >= 0) {
			Invoke ("StartFadeIn", startDelay);
		}

		if (endDelay >= 0) {
			Invoke ("StartFadeOut", endDelay);
		}
	}
}

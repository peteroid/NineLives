using UnityEngine;
using System.Collections;

// add this script to the object which can be controlled by the input
public class InputTest : MonoBehaviour, InputInterface {

	// drag and drop the input to this GameObject
	public GameObject input;

	public void Up () {
	}

	public void Left () {
	}

	public void Right () {
	}

	public void Down () {
	}

	void Start () {
		input.GetComponent<InputScript> ().SetInputInterface (this);
	}
}

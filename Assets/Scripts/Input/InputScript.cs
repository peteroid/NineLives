using UnityEngine;
using UnityEditor;
using System.Collections;

public class InputScript : MonoBehaviour {

	private InputInterface input;
	private string errorInterfaceNotImplemented = "Not implemented interface";
	private string errorObjectNotFound = "Object not found";

	// the user clicked up
	public void Up() {
		Debug.Log ("Up");
		if (input == null) {
			Debug.Log (errorObjectNotFound);
		} else if (! (input is InputInterface)) {
			Debug.Log (errorInterfaceNotImplemented);
		} else {
			((InputInterface) input).Up ();
		}
	}

	// the user clicked left
	public void Left() {
		Debug.Log ("Left");
		if (input == null) {
			Debug.Log (errorObjectNotFound);
		} else if (! (input is InputInterface)) {
			Debug.Log (errorInterfaceNotImplemented);
		} else {
			((InputInterface) input).Left ();
		}
	}

	// the user clicked down
	public void Down() {
		Debug.Log ("Down");
		if (input == null) {
			Debug.Log (errorObjectNotFound);
		} else if (! (input is InputInterface)) {
			Debug.Log (errorInterfaceNotImplemented);
		} else {
			((InputInterface) input).Down ();
		}
	}

	// the user clicked right
	public void Right() {
		Debug.Log ("Right");
		if (input == null) {
			Debug.Log (errorObjectNotFound);
		} else if (! (input is InputInterface)) {
			Debug.Log (errorInterfaceNotImplemented);
		} else {
			((InputInterface) input).Right ();
		}
	}

	public void SetInputInterface (InputInterface input) {
		this.input = input;
	}
}
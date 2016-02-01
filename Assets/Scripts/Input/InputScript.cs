using UnityEngine;
//using UnityEditor;
using System.Collections;

public class InputScript : MonoBehaviour {

	private InputInterface input;
	private bool isEnable = false;
	private string errorInterfaceNotImplemented = "Not implemented interface";
	private string errorObjectNotFound = "Object not found";

	public void SetEnable (bool isEnable) {
		this.isEnable = isEnable;
	}

	// the user clicked up
	public void Up() {
		if (!this.isEnable)
			return;
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
		if (!this.isEnable)
			return;
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
		if (!this.isEnable)
			return;
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
		if (!this.isEnable)
			return;
		if (input == null) {
			Debug.Log (errorObjectNotFound);
		} else if (! (input is InputInterface)) {
			Debug.Log (errorInterfaceNotImplemented);
		} else {
			((InputInterface) input).Right ();
		}
	}

	public void Reset() {
		if (!this.isEnable)
			return;
		Debug.Log ("Reset");
		if (input == null) {
			Debug.Log (errorObjectNotFound);
		} else if (! (input is InputInterface)) {
			Debug.Log (errorInterfaceNotImplemented);
		} else {
			((InputInterface) input).Reset ();
		}
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
			Up ();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
			Down ();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
			Left ();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
			Right ();
        }
		else if (Input.GetKeyDown(KeyCode.R))
		{
			Reset ();
		}
    }

	public void SetInputInterface (InputInterface input) {
		this.input = input;
	}
}
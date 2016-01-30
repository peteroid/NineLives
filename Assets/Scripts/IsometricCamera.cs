using UnityEngine;
using System.Collections;

public class IsometricCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.Rotate(Vector3.forward * -45.0f);
        transform.position = new Vector3(3, 3, -10);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour, InputInterface {

	// drag and drop the input to this GameObject
	public GameObject input;

	// the shorthands are messed up due to the rotation of camera
	public void Up () {
		this.transform.position += Vector3.right;
	}

	public void Left () {
		this.transform.position += Vector3.up;
	}

	public void Right () {
		this.transform.position += Vector3.down;
	}

	public void Down () {
		this.transform.position += Vector3.left;
	}

	// Use this for initialization
	void Start () {
		input.GetComponent<InputScript> ().SetInputInterface (this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

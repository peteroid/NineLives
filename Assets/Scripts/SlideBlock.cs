using UnityEngine;
using System.Collections;

public class SlideBlock : MonoBehaviour {

	public Vector3 velocity;
	private Vector3 startPosition;
	private float enterSpeed;
	private bool isLeaving = false;
	private bool isEntering = false;

	public void SetStartPosition (Vector3 position, float speed)
	{
		this.startPosition = position;
		this.enterSpeed = speed;
		isEntering = true;
	}

	public void SetVelocity (Vector3 v3)
	{
		this.velocity = v3;
		isLeaving = true;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (isLeaving) {
			this.transform.position += (this.velocity * Time.deltaTime);
		} else if (isEntering) {
			this.transform.position = Vector3.MoveTowards (
				this.transform.position,
				startPosition,
				enterSpeed * Time.deltaTime);
			if (this.transform.position == startPosition)
				isEntering = false;
		}
	}
}

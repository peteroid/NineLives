using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour, InputInterface {

	// drag and drop the input to this GameObject
	public GameObject input;
    public TileSystem navGrid;

    private int mX;
    private int mY;

    private bool mInitialized = false;

    private void TryMove(int x, int y)
    {
        if (navGrid.TryMove(mX, mY, mX + x, mY + y))
        {
            mX += x;
            mY += y;

            Vector3 transform = new Vector3(y, x * -1);
            this.transform.position += transform;
        }
    }

	// the shorthands are messed up due to the rotation of camera
	public void Up () {
        TryMove(0, 1);
	}

	public void Left ()
    {
        TryMove(-1, 0);
	}

	public void Right ()
    {
        TryMove(1, 0);
	}

	public void Down ()
    {
        TryMove(0, -1);
	}

	// Use this for initialization
	void Start () {
        mX = 0;
        mY = 0;

		input.GetComponent<InputScript> ().SetInputInterface (this);
	}

    void PostStart()
    {
        // The nav grid has now loaded
        this.transform.position += navGrid.mDisplayOffset;
        TryMove(2, 2);
    }
	
	// Update is called once per frame
	void Update () {
	    if(mInitialized == false)
        {
            mInitialized = true;
            PostStart();
        }
	}
}

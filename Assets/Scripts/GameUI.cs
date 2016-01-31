using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

    Text m_Level;
    Text m_Message;
    Text m_Story;
    Text m_DoorStatus;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UpdateLevelText(int lvlNum)
    {
        UnityEngine.UI.Text text = m_Level.GetComponent<Text>();
        m_Level.text = "Level " + lvlNum;
    }

    public void UpdateMessage(string message)
    {
        UnityEngine.UI.Text text = m_Message.GetComponent<Text>();
        m_Message.text = message;
    }

    public void StoryText()
    {
        UnityEngine.UI.Text text = m_Story.GetComponent<Text>();
        m_Story.text = "You are a cat, seeking to become human! Through the power of magic, will you transform into a human. Solve strange puzzles throughout your journey on becoming a human. Collect golden humans to become human!";
    }
}

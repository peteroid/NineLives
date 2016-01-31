using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

    public Text m_Level;
    public Text m_Message;
    public Text m_Story;
    public Text m_DoorStatus;

	// Use this for initialization
	void Start () {
        UpdateLevelText(1);
        UpdateMessage("Solve the Puzzle!");
        StoryText();
        printDoorStatus(false);
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

    public void printDoorStatus(bool isOpen)
    { 
        UnityEngine.UI.Text text = m_DoorStatus.GetComponent<Text>();
        if (isOpen)
            m_DoorStatus.text = "The door is open!";
        else
            m_DoorStatus.text = "The door is closed.";


    }
}

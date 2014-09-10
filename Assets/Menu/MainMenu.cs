using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public GUIStyle titleStyle;

	private Rect startButtonRect;
    //private Rect titleRect;

	// Use this for initialization
	void Start ()
    {
		int startButtonWidth = 100;
		int startButtonHeight = 50;
		startButtonRect = new Rect(Screen.width/2.0f - startButtonWidth/2.0f, Screen.height/2.0f - startButtonHeight/2.0f, startButtonWidth, startButtonHeight);
        //titleRect = new Rect(Screen.width/2.0f - Screen.width/4.0f, 100, Screen.width/2.0f, startButtonHeight);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("P1 A") || Input.GetButtonDown("P2 A") || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.RightControl))
            Application.LoadLevel("Game");
	}
	
	void OnGUI()
    {
		if(GUI.Button(startButtonRect, "Start"))
        {
			Application.LoadLevel("Game");
		}
	}
}

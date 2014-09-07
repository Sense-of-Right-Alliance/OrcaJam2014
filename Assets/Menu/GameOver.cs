using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

	public GUIStyle titleStyle;
	
	private Rect startButtonRect;
	private Rect titleRect;
	// Use this for initialization
	void Start () {
		int startButtonWidth = 100;
		int startButtonHeight = 50;
		startButtonRect = new Rect(Screen.width/2.0f - startButtonWidth/2.0f, Screen.height/2.0f - startButtonHeight/2.0f, startButtonWidth, startButtonHeight);
		titleRect = new Rect(Screen.width/2.0f - Screen.width/4.0f, 100, Screen.width/2.0f, startButtonHeight);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown ("P1 A")) Application.LoadLevel("MainMenu");
	}
	
	void OnGUI() {
		GUI.Label (titleRect, "game is done now play some more.", titleStyle);
		
		
		if(GUI.Button(startButtonRect, "Restart")) {
			Application.LoadLevel("MainMenu");
		}
	}
}

using UnityEngine;
using System.Collections;

public class GameData : MonoBehaviour {

	public int NumPlayers = 2;
	
	private bool[] deadPlayers;

	// Use this for initialization
	void Start () {
		deadPlayers = new bool[NumPlayers];
		for(int i = 0; i < NumPlayers; i++) {
			deadPlayers[i] = false;
		}
	}
	
	public void RegisterPlayerDead(int id) {
		deadPlayers[id] = true;
		
		int deadCount = 0;
		// check if only one left
		for(int i = 0; i < NumPlayers; i++) {
			if(deadPlayers[i] == true) deadCount += 1;
		}
		
		Debug.Log ("Dead Count = " + deadCount);
		
		if(deadCount >= NumPlayers-1) {
			Application.LoadLevel("End");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

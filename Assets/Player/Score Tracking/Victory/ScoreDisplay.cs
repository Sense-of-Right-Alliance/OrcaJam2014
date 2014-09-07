using UnityEngine;
using System.Collections;

public class ScoreDisplay : MonoBehaviour {

	public Texture2D[] Textures;

	// Use this for initialization
	void Start () {
	
	}
	
	public void UpdateDisplay(int score) {
		if(score == 0) {
			renderer.enabled = false;
		} else if(score < Textures.Length){
			renderer.enabled = true;
			renderer.material.mainTexture = Textures[score-1];
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

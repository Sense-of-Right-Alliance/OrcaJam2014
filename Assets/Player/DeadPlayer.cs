using UnityEngine;
using System.Collections;

public class DeadPlayer : MonoBehaviour {

	public Texture2D[] PlayerTextures;
	
	private int ID;

	// Use this for initialization
	void Start () {
	
	}
	
	public void SetTexture(int id) {
		ID = id;
		GetComponent<Renderer>().material.mainTexture = PlayerTextures[id];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter2D(Collision2D collision) {
		if(collision.gameObject.tag == "Floor") {
			GameObject.Find ("Game Data").GetComponent<GameData>().RegisterPlayerDead(ID);
		}
	}
}

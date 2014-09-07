using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour {

	public int ID = -1; // 1 - 4
	

	// Use this for initialization
	void Start () {
	
	}
	
	public void Initialize(int id, Vector2 startPosition) {
		ID = id;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

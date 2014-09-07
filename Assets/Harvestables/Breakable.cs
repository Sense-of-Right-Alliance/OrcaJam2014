using UnityEngine;
using System.Collections;

/*
A Breakable is a collection of pieces. These pieces include a base, and a number of objects that can be broken off.
The pieces will be broken off one by one with successive punches.
*/
public class Breakable : MonoBehaviour {

	public GameObject[] pieces;		// Break off one by one
	public GameObject basePiece;	// doesn't move, isn't collidable
	
	private int hitCount = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	public void HandlePunch(GameObject player, Vector2 direction) {
		if(hitCount < pieces.Length) {
			GameObject piece = pieces[hitCount]; // Get the piece
			
			piece.GetComponent<Collectable>().BreakOff(direction);
			
			hitCount += 1;
		}
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

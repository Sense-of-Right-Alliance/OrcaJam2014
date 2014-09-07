using UnityEngine;
using System.Collections;

public class BreakableGen : MonoBehaviour {

	public GameObject PiecePrefab;

	public Texture2D[] BrokenStateTextures;
	public Texture2D[] PieceTextures;
	
	private int hitCount = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	public void HandlePunch(GameObject player, Vector2 direction) {
		if(hitCount < BrokenStateTextures.Length) {
			
			
			Vector3 piecePos = transform.position;
			piecePos.y += 2.0f - (BrokenStateTextures.Length - hitCount);
			GameObject piece = (GameObject)Instantiate(PiecePrefab, piecePos, Quaternion.identity);
			piece.renderer.material.mainTexture = PieceTextures[hitCount];
			piece.GetComponent<Collectable>().BreakOff(direction);
			
			renderer.material.mainTexture = BrokenStateTextures[hitCount];
			
			hitCount += 1;
		}
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

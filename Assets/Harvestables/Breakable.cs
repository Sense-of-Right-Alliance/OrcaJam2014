using UnityEngine;
using System.Collections;

public class Breakable : MonoBehaviour {

	public GameObject[] BrokenStateObjectPrefabs;
	public GameObject[] PieceObjects;
	
	public int hitCount = 0;

	// Use this for initialization
	void Start () {
	
	}

    public void HandleBreakage(Vector2 direction)
    {
		if(hitCount < PieceObjects.Length) {
			
			Vector3 piecePos = transform.position;
			piecePos.y += 2.0f - hitCount;
			GameObject piece = (GameObject)Instantiate(PieceObjects[hitCount], piecePos, Quaternion.identity);
			piece.GetComponent<Collectable>().BreakOff(direction);
			
			GameObject newStatue = (GameObject)Instantiate(BrokenStateObjectPrefabs[hitCount], transform.position, transform.rotation);
			float offset = 0.0f;
			if(hitCount == 0) offset -= 0.375f;
			if(hitCount == 1) offset -= 0.283f;
			Vector3 pos = newStatue.transform.position;
			pos.y += offset;
			
			newStatue.transform.position = pos;
			
			newStatue.GetComponent<Breakable>().hitCount = hitCount+1;
			
			Destroy(gameObject);
			//renderer.material.mainTexture = PieceTextures[hitCount];
			
			//hitCount += 1;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

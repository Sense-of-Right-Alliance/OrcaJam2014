using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Collector : MonoBehaviour {

	public float CollectionOffset = 0.5f;
	public Transform StartingCollectablePosition;

	public List<GameObject> collectedObjects = new List<GameObject>();
	
	public AudioClip CollectSound;
	public AudioClip DropSound;
	
	private GameObject scoreObject;
	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		scoreObject = transform.FindChild("Score").gameObject;
		
		scoreObject.GetComponent<ScoreDisplay>().UpdateDisplay(0);
		
		audioSource = Camera.main.GetComponent<AudioSource>();
	}
	
	public void Collect(GameObject obj) {
		
		Collectable collectable = obj.GetComponent<Collectable>();
		collectable.StackTo(GetNextCollectablePosition(), gameObject);
		
		audioSource.PlayOneShot (CollectSound);
	}
	
	// called after an object is set on the head
	public void HandleCollected(GameObject obj) {
		obj.collider2D.enabled = false;
		obj.renderer.enabled = false;
		collectedObjects.Add (obj);
		scoreObject.GetComponent<ScoreDisplay>().UpdateDisplay(collectedObjects.Count);
		
	
		int collectedCount = 0;
		for(int i = 0; i < collectedObjects.Count; i++) {
			if(collectedObjects[i].GetComponent<Collectable>().collected) collectedCount += 1;
		}
		
		if(collectedCount >= 2) {
			Application.LoadLevel("End");
		}
	}
	
	public void KnockOffBlock(Vector2 direction) {
		if(collectedObjects.Count > 0) {
			GameObject topObject = collectedObjects[collectedObjects.Count-1];
			
			topObject.collider2D.enabled = true;
			topObject.renderer.enabled = true;
			
			topObject.GetComponent<Collectable>().BreakOff (direction);
			
			collectedObjects.Remove (topObject);
			scoreObject.GetComponent<ScoreDisplay>().UpdateDisplay(collectedObjects.Count);
			
			audioSource.PlayOneShot (DropSound);
		}
	}
	
	public void TakeHit(GameObject obj) {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	Vector3 GetNextCollectablePosition() {
		Vector3 pos = StartingCollectablePosition.position;
		pos.y += collectedObjects.Count * CollectionOffset;
		
		return pos;
	}
}

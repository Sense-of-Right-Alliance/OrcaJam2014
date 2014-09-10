using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Collector : MonoBehaviour {

	public float CollectionOffset = 0.5f;
	public Transform StartingCollectablePosition;

	public List<GameObject> collectedObjects = new List<GameObject>();
	
	public AudioClip CollectSound;
	public AudioClip DropSound;
	
	public Texture2D[] FaceTextures;

    private GameData gameData;
    private PlayerData playerData;
	public GameObject DeadPlayer;
	
	private GameObject scoreObject;
	private AudioSource audioSource;
	
	private float hitTimer = 0.0f;
	
	private bool isDead = false;

	// Use this for initialization
	void Start () {
		scoreObject = transform.FindChild("Score").gameObject;
		scoreObject.GetComponent<ScoreDisplay>().UpdateDisplay(0);
		audioSource = Camera.main.GetComponent<AudioSource>();

        playerData = GetComponent<PlayerData>();
        gameData = GameObject.Find("Game Data").GetComponent<GameData>();
	}
	
	public void Collect(GameObject obj) {
		if(!isDead) {
			Collectable collectable = obj.GetComponent<Collectable>();
			collectable.StackTo(GetNextCollectablePosition(), gameObject);
			
			//HandleCollected(obj);
			
			audioSource.PlayOneShot (CollectSound);
		}
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
		
		if(collectedCount >= 5)
        {
            gameData.winningPlayerID = playerData.ID;
			Application.LoadLevel("End");
		}
	}
	
	public void KnockOffBlock(Vector2 direction) {
		if(collectedObjects.Count > 0) {
			GameObject topObject = collectedObjects[collectedObjects.Count-1];
			
			topObject.collider2D.enabled = true;
			topObject.renderer.enabled = true;
			
			Vector3 pos = topObject.transform.position;
			pos.y += 0.5f;
			topObject.transform.position = pos;
			topObject.GetComponent<Collectable>().BreakOff (direction);
			
			collectedObjects.Remove (topObject);
			scoreObject.GetComponent<ScoreDisplay>().UpdateDisplay(collectedObjects.Count);
			
			audioSource.PlayOneShot (DropSound);
		} else {
            var deadPlayer = (GameObject)Instantiate(DeadPlayer, transform.position, transform.rotation);
            var deadPlayerScript = deadPlayer.GetComponent<DeadPlayer>();
            deadPlayerScript.SetTexture(playerData.ID);
			
			Destroy(gameObject);
		}
	}
	
	public void TakeHit(GameObject obj) {
		if(!isDead) {
			KnockOffBlock(-obj.transform.up);
			
			transform.FindChild ("Head").GetComponent<Renderer>().material.mainTexture = FaceTextures[1];
			hitTimer = 0.5f;
			
			
		}
	}
	
	public void TakeHitDir(Vector2 dir) {
		if(!isDead) {
			KnockOffBlock(dir);
			
			transform.FindChild ("Head").GetComponent<Renderer>().material.mainTexture = FaceTextures[1];
			hitTimer = 0.5f;
		}
	}
	
	void CheckDead(Vector2 dir) {
		if(collectedObjects.Count == 0) {
            var deadPlayer = (GameObject)Instantiate(DeadPlayer, transform.position, transform.rotation);
            var deadPlayerScript = deadPlayer.GetComponent<DeadPlayer>();
            deadPlayerScript.SetTexture(playerData.ID);
			
			Destroy(gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(hitTimer > 0.0f) {
			hitTimer -= Time.deltaTime;
			if(hitTimer <= 0.0f) {
				transform.FindChild ("Head").GetComponent<Renderer>().material.mainTexture = FaceTextures[0];
			}
		}
	}
	
	Vector3 GetNextCollectablePosition() {
		Vector3 pos = StartingCollectablePosition.position;
		pos.y += collectedObjects.Count * CollectionOffset;
		
		return pos;
	}
	
	void OnCollisionEnter2D(Collision2D collision) {

	}
}

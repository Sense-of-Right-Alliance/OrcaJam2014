using UnityEngine;
using System.Collections;

public class Collectable : MonoBehaviour {

	public bool canCollect = false;
	public bool collected = false;
	public bool collecting = false;
	
	private Vector3 targetPosition;
	private GameObject collectingPlayer;
	
	private Vector3 velocity;
	private float acceleration = 1.0f;
	private float maxSpeed = 5.0f;
	
	//private Rigidbody2D rigidbody;

	// Use this for initialization
	void Start () {
		//rigidbody = GetComponent<Rigidbody2D>();
	}
	
	public void StackTo(Vector3 pos, GameObject player) {
		collectionTimer = 0.0f;
	
		collectingPlayer = player;
		
		collectingPlayer.GetComponent<PlayerMovement>().enabled = false;
	
		transform.parent = collectingPlayer.transform;
		
		GetComponent<BoxCollider2D>().isTrigger = true;
		rigidbody2D.isKinematic = true;
		targetPosition = pos;
		
		collecting = true;
		canCollect = false;
		
		Vector2 pDir = collectingPlayer.GetComponent<PlayerMovement>().direction;
		Vector3 dir = new Vector3(pDir.x, pDir.y, 0);
		velocity = ((dir * 0.5f) + collectingPlayer.transform.up).normalized * maxSpeed;
		
		FinishStackTo();
	}
	
	public void BreakOff(Vector2 direction) {
		transform.parent = null; // remove parent
		
		Vector3 pos = transform.position;
		pos.y += 0.5f;
		transform.position = pos;
		
		// Launch the piece
		GetComponent<BoxCollider2D>().isTrigger = false;
		rigidbody2D.isKinematic = false;
		// Calc horizontal offset
		Vector2 randomOffset = transform.right * Random.value * 200.0f;
		if(Random.value < 0.5f) randomOffset *= -1;
		float verticalPower = 60.0f * (Random.value * 5.0f);
		
		Vector2 force = new Vector2(transform.up.x, transform.up.y) * verticalPower + direction * 10.0f;// randomOffset;
		rigidbody2D.AddForce(force);
		
		// Enable collection of the piece
		canCollect = true;
		collecting = false;
		collected = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(collecting) UpdateCollection();
	}
	
	private float collectionTimer = 0.0f;
	void UpdateCollection() {
		collectionTimer += Time.deltaTime;
		
		Vector3 dir = (targetPosition - transform.position);
		float distance = dir.magnitude;
		dir.z = 0;
		dir = dir.normalized;
		
		velocity += dir * acceleration * (collectionTimer);
		
		float max = maxSpeed;
		if(distance < 1.0f) max *= distance;
		max = Mathf.Max (max, 0.1f);
		
		if(velocity.magnitude > max) {
			velocity = velocity.normalized * max;
		}
		
		transform.position += velocity * Time.deltaTime;
		
		if((targetPosition - transform.position).magnitude <= 0.05f) {
			FinishStackTo();
		}
	}
	
	void FinishStackTo() {
		transform.position = targetPosition;
		
		collectingPlayer.GetComponent<PlayerMovement>().enabled = true;
		collecting = false;
		collected = true;
		
		collectingPlayer.GetComponent<Collector>().HandleCollected(gameObject);
	}
	
	void OnCollisionEnter2D(Collision2D collision) {
		if(canCollect && !collected && collision.gameObject.tag == "Player") {
			
			Debug.Log ("Player " + collision.gameObject.GetComponent<PlayerData>().ID + " Collected");
			collision.gameObject.GetComponent<Collector>().Collect (gameObject);
		}
	}
}

using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float moveSpeed = 5000.0f;
	public float jumpSpeed = 50000.0f;
	
	public Vector2 direction;

	private Rigidbody2D rigidBody2D;
	
	private bool isGrounded = false;

	// Use this for initialization
	void Start () {
		rigidBody2D = GetComponent<Rigidbody2D>();
		
		direction = transform.right;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.A)) {
			rigidBody2D.AddForce(-transform.right * moveSpeed * Time.deltaTime); // Move with force to left
			direction = -transform.right; // Set facing direction to left
		}
		
		if(Input.GetKey(KeyCode.D)) {
			rigidBody2D.AddForce(transform.right * moveSpeed * Time.deltaTime); // Move with force to right
			direction = transform.right; // Set facing direction to right
		}
		
		if(isGrounded && Input.GetKeyDown(KeyCode.W)) {
			rigidBody2D.AddForce(transform.up * jumpSpeed * Time.deltaTime);
			isGrounded = false;
		}
	}
	
	void OnCollisionEnter2D(Collision2D collision) {
		if(collision.gameObject.tag == "Floor") {
			isGrounded = true;
		}
	}
}

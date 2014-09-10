﻿using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float moveSpeed = 5000.0f;
	public float jumpSpeed = 50000.0f;
	
	public Vector2 direction;
	
	public AudioClip[] FootstepSounds;

	private PlayerData playerData;
	private Animator animator;
	private AudioSource audioSource;

    private int ID;
    private int inputID { get { return ID + 1; } }
	
	private bool isGrounded = true;
	private float jumpTime = 0.12f;
	private float jumpTimer = 0.0f; // the amount of time force is applied for jumping
	
	private float landTime = 0.05f;
	private float landTimer = 0.0f;
	
	private float soundDelay = 0.0f;
	
    //private bool isFalling = true;

	// Use this for initialization
	void Start () {
		playerData = GetComponent<PlayerData>();
		animator = GetComponent<Animator>();
		audioSource = Camera.main.GetComponent<AudioSource>();
		
		ID = playerData.ID;
		
		direction = transform.right;
	}
	
	// Update is called once per frame
	void Update () {
		if(landTimer <= 0.0f) {
			HandleGamepadInput();
			HandleKeyboardInput();
		}
		
		/*if(rigidbody2D.velocity.y <= 0) isFalling = true;
		else {
			isFalling = false;
			if(!isGrounded && rigidbody2D.velocity.y == 0) {
				isGrounded = true;
				animator.SetBool ("Jump", false);
				animator.SetBool("Land",true);
				
				landTimer = landTime;
			}
		}*/
		
		animator.SetFloat("Speed", rigidbody2D.velocity.magnitude);
		
		if(soundDelay > 0.0f) soundDelay -= Time.deltaTime;
		
		if(isGrounded && rigidbody2D.velocity.magnitude > 0.1f && soundDelay <= 0.0f) {
			audioSource.PlayOneShot (FootstepSounds[Random.Range(0,FootstepSounds.Length)]);
			soundDelay = 0.2f;
		}
		
		if(jumpTimer > 0.0f) {
			jumpTimer -= Time.deltaTime;
		}
		
		if(landTimer > 0.0f) {
			landTimer -= Time.deltaTime;
			animator.SetBool ("Land", false);
		}
	}
	
	void HandleGamepadInput() {
		// Movment
		float horz = Input.GetAxis ("P" + inputID.ToString() + " Left Stick Horizontal");
        //float vert = Input.GetAxis ("P" + inputID.ToString() + " Left Stick Vertical");		// dont' really care about this... FOR NOW!
		
		rigidbody2D.AddForce(transform.right * moveSpeed * horz); // Move with force to left
		
		if(horz != 0.0f) direction = (transform.right * horz).normalized; // Set facing direction to left
		
		// set animation direction
        if ((horz < 0 && transform.localScale.x < 0) || (horz > 0 && transform.localScale.x > 0))
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
		
		// Jump
        if (Input.GetButton("P" + inputID.ToString() + " A") && (isGrounded || (!isGrounded && jumpTimer > 0.0f)))
        {
			Debug.Log ("Jumping");
			
			float jSpeedMod = jumpSpeed;
			if(!isGrounded) jSpeedMod *= 0.3f;
			rigidbody2D.AddForce(transform.up * jSpeedMod);
			
			if(isGrounded) { // start of jump
				jumpTimer = jumpTime;
				animator.SetBool("Jump", true);
			}
			isGrounded = false;
		}
	}
	
	void HandleKeyboardInput()
    {
        KeyCode keyLeft = KeyCode.A;
        KeyCode keyRight = KeyCode.D;
        KeyCode keyJump = KeyCode.W;

        if (ID == 1)
        {
            keyLeft = KeyCode.LeftArrow;
            keyRight = KeyCode.RightArrow;
            keyJump = KeyCode.UpArrow;
        }

        // Left Movement
        if (Input.GetKey(keyLeft))
        {
            if (transform.localScale.x < 0)
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            rigidbody2D.AddForce(-transform.right * moveSpeed); // Move with force to left
            direction = -transform.right; // Set facing direction to left
        }

		// Right Movement
        if (Input.GetKey(keyRight))
        {
            if (transform.localScale.x > 0)
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            rigidbody2D.AddForce(transform.right * moveSpeed); // Move with force to right
            direction = transform.right; // Set facing direction to right
        }

		// Jump
        if (Input.GetKey(keyJump) && (isGrounded || (!isGrounded && jumpTimer > 0.0f)))
        {
            Debug.Log("Jumping");

            float jSpeedMod = jumpSpeed;
            if (!isGrounded) jSpeedMod *= 0.3f;
            rigidbody2D.AddForce(transform.up * jSpeedMod);

            if (isGrounded)
            { // start of jump
                jumpTimer = jumpTime;
                animator.SetBool("Jump", true);
            }
            isGrounded = false;
        }
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == "Player" || collision.gameObject.tag == "Breakable" || collision.gameObject.tag == "Floor") && !isGrounded)
        {
            var relativePosition = transform.InverseTransformPoint(collision.contacts[0].point);

            if (relativePosition.y < 0)
            {
                Debug.Log("The object is below.");
                isGrounded = true;
                animator.SetBool("Jump", false);
                animator.SetBool("Land", true);

                landTimer = landTime;
            }
        }
    }
}

using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour {

	private static float COOLDOWN = 0.3f;
	
	public float reach = 1.0f;
	public float radius = 0.5f;
	
	public AudioClip[] HitSounds;
	public AudioClip[] PokeSounds;
	
	public GameObject ArrowFab;
	public GameObject StalactiteBreakEffect;

	private PlayerMovement playerMovement;
	private PlayerData playerData;
	private Animator animator;
	private AudioSource audioSource;

    private int ID;
    private short inputID { get { return (short)(ID + 1); } }
	
	private bool canPunch = false;
	private float cooldownTimer = 0.0f;
	
	private string hitType = "";
	
	//private float hitDelay = 0.3f;
	//private bool hitBuffered = false;
	//private GameObject hitObj;
	
	private float hitThreshold = 0.0f;

	// Use this for initialization
	void Start () {
		playerMovement = transform.parent.gameObject.GetComponent<PlayerMovement>();
		playerData = transform.parent.gameObject.GetComponent<PlayerData>();
		animator = transform.parent.gameObject.GetComponent<Animator>();
		audioSource = Camera.main.GetComponent<AudioSource>();
		
		ID = playerData.ID;
		
		GetComponent<BoxCollider2D>().enabled = false;
	}
	
	// Update is called once per frame
	void Update()
    {
		HandleCooldown();
		
		//if(hitDelay > 0.0f) hitDelay -= Time.deltaTime;
		//if(hitDelay <= 0.0f && hitBuffered) MakeHit();
		
		if(hitThreshold > 0.0f) hitThreshold -= Time.deltaTime;
		else GetComponent<BoxCollider2D>().enabled = false;
	
		if(canPunch)
        {
            KeyCode keyPunch = KeyCode.Space;
            KeyCode keyLob = KeyCode.LeftShift;

            if (ID == 1)
            {
                keyPunch = KeyCode.RightControl;
                keyLob = KeyCode.RightShift;
            }

            if (Input.GetKeyDown(keyPunch) || Input.GetButtonDown("P" + inputID.ToString() + " B"))
            {
				canPunch = false;
				cooldownTimer = COOLDOWN;
				
				MakeHit("Straight");

                audioSource.PlayOneShot(PokeSounds[Random.Range(0, PokeSounds.Length)]);
                animator.SetTrigger("Poke");
            }
            else if (Input.GetKeyDown(keyLob) || Input.GetButtonDown("P" + inputID.ToString() + " Y"))
            { 	// Lob
				MakeHit("Lob");
            }
            //else if (Input.GetButtonDown("P" + inputID.ToString() + " X"))
            //{	// Downward Shots
            //    MakeHit("Down");
            //}
		}
		
		Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y, -10) + new Vector3(playerMovement.direction.x, playerMovement.direction.y,0) * reach);
	}
	
	// Called after delay from key stroke to sync up with animation
	void MakeHit(string type) {
		GetComponent<BoxCollider2D>().enabled = true;
		hitThreshold = 0.1f;
		hitType = type;
	}
	
	void HandleCooldown() {
		if(!canPunch) {
			cooldownTimer -= Time.deltaTime;
			if(cooldownTimer <= 0.0f) canPunch = true;
		}
	}
	
	Vector3 GetTrajectory() {
		switch(hitType) {
			case "Down":
				return new Vector3(0.0f,-1.0f,0.0f) * 1000.0f;
			case "Lob":
				return ((Vector3)playerMovement.direction + new Vector3(0.0f, 1.0f, 0.0f)).normalized * 500.0f;
			case "Straight": 
			default:
				return (Vector3)playerMovement.direction * 2500.0f;
		}
	}
	
	Vector3 GetArrowOffset() {
		switch(hitType) {
			case "Down":
				return new Vector3(0.0f,-1.0f,0.0f) * 0.5f;
			case "Lob":
				return ((Vector3)playerMovement.direction + new Vector3(0.0f, 1.0f, 0.0f)).normalized * 0.5f;
			case "Straight": 
			default:
				return (Vector3)playerMovement.direction * 0.5f;
		}
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        //if(hitThreshold > 0.0f) {
        if (collision.gameObject.tag == "Breakable")
        {
            Debug.Log("Object Broken");

            if (collision.gameObject.GetComponent<Breakable>() != null)
                collision.gameObject.GetComponent<Breakable>().HandleBreakage(playerMovement.direction);

            audioSource.PlayOneShot(HitSounds[Random.Range(0, HitSounds.Length)]);

            hitThreshold = 0.0f;
        }
        //else if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<PlayerData>().ID != ID)
        //{
        //    Debug.Log("Player Hit");

        //    Collector collector = collision.gameObject.GetComponent<Collector>();
        //    collector.KnockOffBlock(playerMovement.direction);

        //    audioSource.PlayOneShot(HitSounds[Random.Range(0, HitSounds.Length)]);

        //    hitThreshold = 0.0f;
        //}
        else if (collision.gameObject.tag == "Stalactite")
        {
            Debug.Log("Stalactite Poked");
            //Debug.Log ("ArrowPrefab = " + ArrowFab + " playerMovement = " + playerMovement);

            GameObject arrow = (GameObject)Instantiate(ArrowFab, transform.parent.FindChild("Hand").position + GetArrowOffset(), transform.rotation);

            Instantiate(StalactiteBreakEffect, transform.position, transform.rotation);

            if (playerMovement.direction.x > 0.0f)
            {
                Vector3 scale = arrow.transform.localScale;
                scale.x *= -1.0f;

                arrow.transform.localScale = scale;
            }

            Destroy(collision.gameObject);
            arrow.rigidbody2D.AddForce(GetTrajectory());

            audioSource.PlayOneShot(HitSounds[Random.Range(0, HitSounds.Length)]);
        }
    }
}

using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour {

	private static float COOLDOWN = 0.3f;
	
	public float reach = 1.0f;
	public float radius = 0.5f;
	
	public AudioClip[] HitSounds;
	public AudioClip[] PokeSounds;

	private PlayerMovement playerMovement;
	private PlayerData playerData;
	private Animator animator;
	private AudioSource audioSource;
	
	private int ID;
	
	private bool canPunch = false;
	private float cooldownTimer = 0.0f;
	
	private float hitDelay = 0.3f;
	private bool hitBuffered = false;
	private GameObject hitObj;

	// Use this for initialization
	void Start () {
		playerMovement = GetComponent<PlayerMovement>();
		playerData = GetComponent<PlayerData>();
		animator = GetComponent<Animator>();
		audioSource = Camera.main.GetComponent<AudioSource>();
		
		ID = playerData.ID;
	}
	
	// Update is called once per frame
	void Update () {
		HandleCooldown();
		
		if(hitDelay > 0.0f) hitDelay -= Time.deltaTime;
		if(hitDelay <= 0.0f && hitBuffered) MakeHit();
	
		if(canPunch && Input.GetKeyDown (KeyCode.Space) || Input.GetButtonDown("P" + ID.ToString() + " B")) {
			canPunch = false;
			cooldownTimer = COOLDOWN;
			
			Vector2 center = new Vector2(transform.position.x, transform.position.y) + playerMovement.direction * reach;
			Vector3 actualCenter = new Vector3(center.x, center.y, -10);
			Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center,radius);
			for(int i = 0; i < hitColliders.Length; i++) {
				if(hitColliders[i].tag == "Breakable") {
					Debug.Log("Object Broken");
					hitObj = hitColliders[i].gameObject;
					
					hitDelay = 0.2f;
					hitBuffered = true;
					
					//obj.GetComponent<BreakableGen>().HandlePunch(gameObject, playerMovement.direction);
				} else if(hitColliders[i].tag == "Player" && hitColliders[i].gameObject.GetComponent<PlayerData>().ID != ID) {
					Debug.Log("Player Hit");
					hitObj = hitColliders[i].gameObject;
					
					hitDelay = 0.2f;
					hitBuffered = true;
					
					//Collector collector = obj.GetComponent<Collector>();
					//collector.KnockOffBlock(playerMovement.direction);
				}
			}
			
			audioSource.PlayOneShot (PokeSounds[Random.Range(0,PokeSounds.Length)]);
			animator.SetTrigger ("Poke");
		}
		
		Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y, -10) + new Vector3(playerMovement.direction.x, playerMovement.direction.y,0) * reach);
	}
	
	// Called after delay from key stroke to sync up with animation
	void MakeHit() {
		if(hitObj.tag == "Breakable") {
			Debug.Log("Object Broken");
			
			hitDelay = 0.2f;

            hitObj.GetComponent<Breakable>().HandleBreakage(playerMovement.direction);
		} else if(hitObj.tag == "Player" && hitObj.GetComponent<PlayerData>().ID != ID) {
			Debug.Log("Player Hit");
			
			hitDelay = 0.2f;
			
			Collector collector = hitObj.GetComponent<Collector>();
			collector.KnockOffBlock(playerMovement.direction);
		}
		
		audioSource.PlayOneShot (HitSounds[Random.Range(0,HitSounds.Length)]);
		
		hitBuffered = false;
	}
	
	void HandleCooldown() {
		if(!canPunch) {
			cooldownTimer -= Time.deltaTime;
			if(cooldownTimer <= 0.0f) canPunch = true;
		}
	}
}

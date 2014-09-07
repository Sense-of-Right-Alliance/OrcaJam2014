using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour {

	private static float COOLDOWN = 0.5f;
	
	public float reach = 1.0f;
	public float radius = 0.5f;

	private PlayerMovement playerMovement;
	private PlayerData playerData;
	
	private int ID;
	
	private bool canPunch = false;
	private float cooldownTimer = 0.0f;

	// Use this for initialization
	void Start () {
		playerMovement = GetComponent<PlayerMovement>();
		playerData = GetComponent<PlayerData>();
		
		ID = playerData.ID;
	}
	
	// Update is called once per frame
	void Update () {
		HandleCooldown();
	
		if(canPunch && Input.GetKeyDown (KeyCode.Space) || Input.GetButtonDown("P" + ID.ToString() + " B")) {
			canPunch = false;
			cooldownTimer = COOLDOWN;
			
			Vector2 center = new Vector2(transform.position.x, transform.position.y) + playerMovement.direction * reach;
			Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center,radius);
			for(int i = 0; i < hitColliders.Length; i++) {
				if(hitColliders[i].tag == "Breakable") {
					Debug.Log("Object Broken");
					GameObject obj = hitColliders[i].gameObject;
					
					obj.GetComponent<BreakableGen>().HandlePunch(gameObject, playerMovement.direction);
				} else if(hitColliders[i].tag == "Player" && hitColliders[i].gameObject.GetComponent<PlayerData>().ID != ID) {
					Debug.Log("Player Hit");
					GameObject obj = hitColliders[i].gameObject;
					
					Collector collector = obj.GetComponent<Collector>();
					collector.KnockOffBlock(playerMovement.direction);
				}
			}
		}
		
		Debug.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y) + playerMovement.direction * reach);
	}
	
	void HandleCooldown() {
		if(!canPunch) {
			cooldownTimer -= Time.deltaTime;
			if(cooldownTimer <= 0.0f) canPunch = true;
		}
	}
}

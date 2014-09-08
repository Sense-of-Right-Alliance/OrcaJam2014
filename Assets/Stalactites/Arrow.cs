using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

	public GameObject ExplodeEffect;

	private float timer = 0.0f;
	private bool fading = false;

	// Use this for initialization
	void Start () {
	
	}
	
	void StartFade() {
		timer = 1.0f;
		fading = true;
		
		GetComponent<BoxCollider2D>().enabled = false;
		GetComponent<Rigidbody2D>().isKinematic = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(fading) {
			timer -= Time.deltaTime;
			if(timer <= 0.0f) {
				Destroy(gameObject);
			}
		}
	}
	
	void OnCollisionEnter2D(Collision2D collision) {
		Debug.Log ("Arrow hit = " + collision.gameObject.tag);
		if(collision.gameObject.tag == "Head") {
			collision.gameObject.transform.parent.GetComponent<Collector>().TakeHitDir(rigidbody2D.velocity);
			
			Instantiate(ExplodeEffect, transform.position, transform.rotation);
		} else if(collision.gameObject.tag == "Player") {
			collision.gameObject.GetComponent<Collector>().TakeHitDir(rigidbody2D.velocity);
			
			Instantiate(ExplodeEffect, transform.position, transform.rotation);
		} else if(collision.gameObject.tag == "Breakable") {
			collision.gameObject.GetComponent<Breakable>().HandleBreakage(rigidbody2D.velocity);
			Instantiate(ExplodeEffect, transform.position, transform.rotation);
		} else if(collision.gameObject.tag == "Floor") {
			StartFade();
		}
	}
}

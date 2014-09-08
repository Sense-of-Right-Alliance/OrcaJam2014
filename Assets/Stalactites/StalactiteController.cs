using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StalactiteController : MonoBehaviour
{
    public GameObject explosionPrefab;
    public float detachGravityScale = 0.1f;

    public enum StalactiteState
    {
        Attached,
        Detaching,
        Falling,
        Breaking,
        Fallen,
    }

    public StalactiteState state;

    // Use this for initialization
    void Start()
    {
        StartDetach();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case StalactiteState.Falling:
                //if (rigidbody2D.IsSleeping())
                //    LayToRest();
                break;
        }
    }

    public void StartDetach()
    {
        if (state == StalactiteState.Attached)
        {
            state = StalactiteState.Detaching;
            // start animated wiggle TODO

            rigidbody2D.gravityScale = detachGravityScale;
        }
    }

    void CompleteDetach()
    {
        state = StalactiteState.Falling;
        // stop animated wiggle TODO
        // play sound? TODO

        rigidbody2D.gravityScale = 1;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
    	Debug.Log ("Stalactite Hit Tag = " + coll.collider.gameObject.tag + " State = " + state);
        switch (coll.collider.gameObject.tag)
        {
            case "Floor":
                if (state == StalactiteState.Falling)
                {
                    //LayToRest();
                    Debug.Log ("Stalactite hit Floor");
					gameObject.layer =  LayerMask.NameToLayer("StuckStalactite");
                    //Explode(transform.position);//coll.contacts.First().point);
                }
                break;

            case "Breakable":
                if (state == StalactiteState.Falling)
					Debug.Log ("Stalactite hit Breakable");
                    LandOnStatue(coll.gameObject.GetComponent<Breakable>(), coll.contacts.First().point);
                break;

            case "Collectable":
                if (state == StalactiteState.Falling)
					Debug.Log ("Stalactite hit Collectable");
                    //LandOnStatuePiece(coll.gameObject.GetComponent<Collectable>(), coll.contacts.First().point);
					Explode(coll.contacts.First().point);
                break;

            case "Head":
                if (state == StalactiteState.Falling || state == StalactiteState.Detaching)
					Debug.Log ("Stalactite hit Head");
                    LandOnPlayer(coll.gameObject.GetComponent<Collector>(), coll.contacts.First().point);
                break;
        }
    }
    
    void OnCollisionExit2D(Collision2D other) {
		switch (other.gameObject.tag)
		{
		case "Ceiling":
			if (state == StalactiteState.Detaching)
				CompleteDetach();
			break;
		}
    }

    void OnTriggerExit2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Ceiling":
                if (state == StalactiteState.Detaching)
                    CompleteDetach();
                break;
        }
    }

    void LandOnStatue(Breakable statue, Vector2 position)
    {
        statue.HandleBreakage(statue.transform.position - transform.position);
        Explode(position);
    }

    void LandOnStatuePiece(Collectable statue, Vector2 position)
    {
        DestroyObject(statue.gameObject);
        Explode(position);
    }

    void LandOnPlayer(Collector player, Vector2 position)
    {
        player.TakeHit(gameObject);
        Explode(position);
    }

    void Explode(Vector2 position)
    {
        // play sound? TODO
        Instantiate(explosionPrefab, new Vector3(position.x, position.y, -11), Quaternion.identity);
        Destroy(gameObject);
    }

    void LayToRest()
    {
        state = StalactiteState.Fallen;
        SetCollidingEnabled(false);
        SetGravityEnabled(false);
    }

    void SetCollidingEnabled(bool collide)
    {
        collider2D.isTrigger = !collide;
    }

    void SetGravityEnabled(bool gravity)
    {
        rigidbody2D.isKinematic = !gravity;
    }
}


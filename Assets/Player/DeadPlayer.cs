using UnityEngine;
using System.Collections;

public class DeadPlayer : MonoBehaviour
{
	public Texture2D[] PlayerTextures;
    private int ID;
    private GameData gameData;
    bool registeredDead = false;
    float maxTimeUndead = 2.0f;
    float timeUndead = 0;

	// Use this for initialization
	void Start()
    {
        gameData = GameObject.Find("Game Data").GetComponent<GameData>();
	}

    public void SetTexture(int id)
    {
		ID = id;
		GetComponent<Renderer>().material.mainTexture = PlayerTextures[id];
	}
	
	// Update is called once per frame
	void Update()
    {
        if (!registeredDead && rigidbody2D.IsSleeping())
        {
            RegisterAsDead();
        }

        timeUndead += Time.deltaTime;
        if (timeUndead > maxTimeUndead)
        {
            RegisterAsDead();
        }
	}

    void RegisterAsDead()
    {
        gameData.RegisterPlayerDead(ID);
        registeredDead = true;
    }
	
	void OnCollisionEnter2D(Collision2D collision)
    {
        //if(collision.gameObject.tag == "Floor")
        //{
        //    gameData.RegisterPlayerDead(ID);
        //}
	}
}

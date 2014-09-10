using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameData : MonoBehaviour
{
    public class Player
    {
        public int ID { get; private set; }
        public bool Alive { get; set; }
        public Player(int id) { this.ID = id; Alive = true; }
    }

	public short NumPlayers = 2;
    private List<Player> players;

    public int? winningPlayerID = null;

	// Use this for initialization
	void Start ()
    {
        players = new List<Player>();
        for (int id = 0; id < NumPlayers; id++)
        {
            players.Add(new Player(id));
        }
		
		Object.DontDestroyOnLoad(gameObject);
	}
	
	public void RegisterPlayerDead(int id)
    {
        var player = players.Where(u => u.ID == id).SingleOrDefault();
        player.Alive = false;

        Debug.Log("Dead Count = " + players.Where(u => !u.Alive).Count());
        
		// check if only one player alive
        var alivePlayers = players.Where(u => u.Alive);

        if (alivePlayers.Count() == 1)
        {
            winningPlayerID = alivePlayers.First().ID;
			Application.LoadLevel("End");
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}

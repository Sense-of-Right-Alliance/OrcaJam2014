using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TimedDestruction : MonoBehaviour
{
    public float timeToLive = 1.0f;

    float timeRemaining = 0;

    // Use this for initialization
    void Start()
    {
        timeRemaining = timeToLive;
    }
    
    // Update is called once per frame
    void Update()
    {
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0)
            DestroyObject(gameObject);
    }
}

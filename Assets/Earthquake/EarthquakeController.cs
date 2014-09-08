using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class EarthquakeController : MonoBehaviour
{
    public float durationInactive = 5.0f;
    public float durationPreEarthquake = 2.0f;
    public float durationMainEarthquakeInitial = 2.0f;
    public float durationMainEarthquakeIncrement = 1f;
    public float stalactiteGenerationInterval = 1f;

    public enum EarthquakeState
    {
        Inactive,
        PreEarthquake,
        MainEarthquake,
    }

    public EarthquakeState state;
    public GameObject stalactitePrefab;

    float durationMainEarthquakeCurrent = 0;
    float timeRemainingInactive = 0;
    float timeRemainingPreEarthquake = 0;
    float timeRemainingMainEarthquake = 0;
    float timeToStalactiteGeneration = 0;

    MeshCollider mesh;

    // Start is called just before any of the Update methods is called the first time (Since v1.0)
    void Start()
    {
        mesh = GetComponent<MeshCollider>();
        durationMainEarthquakeCurrent = durationMainEarthquakeInitial;
        timeRemainingInactive = durationInactive;
    }

    // Update is called every frame, if the MonoBehaviour is enabled (Since v1.0)
    void Update()
    {
        switch (state)
        {
            case EarthquakeState.Inactive:
                timeRemainingInactive -= Time.deltaTime;
                if (timeRemainingInactive <= 0)
                    StartPreEarthquakeEvent();
                break;
            case EarthquakeState.PreEarthquake:
                timeRemainingPreEarthquake -= Time.deltaTime;
                if (timeRemainingPreEarthquake <= 0)
                    StartMainEarthquakeEvent();
                break;
            case EarthquakeState.MainEarthquake:
                timeRemainingMainEarthquake -= Time.deltaTime;
                if (timeRemainingMainEarthquake <= 0)
                    EndEarthquakeEvent();
                break;
        }

        if (state == EarthquakeState.MainEarthquake)
        {
            timeToStalactiteGeneration -= Time.deltaTime;
            if (timeToStalactiteGeneration <= 0)
                GenerateStalactite();
        }
    }

    void GenerateStalactite()
    {
        // generate stalactites within area
        var width = stalactitePrefab.renderer.bounds.size.x;
        var left = mesh.bounds.min.x + width;
        var right = mesh.bounds.max.x - width;
        var range = right - left;

        var x = Random.value * range + left;
        Instantiate(stalactitePrefab, new Vector3(x, transform.position.y, transform.position.z), Quaternion.identity);

        timeToStalactiteGeneration = stalactiteGenerationInterval;
    }

    void StartPreEarthquakeEvent()
    {
    	Debug.Log ("Earthquake Pre");
        state = EarthquakeState.PreEarthquake;
        // play sound TODO
        // gently shake screen TODO
		Camera.main.GetComponent<CameraShake>().ShakeWarmup();
		
        timeRemainingPreEarthquake = durationPreEarthquake;
    }

    void StartMainEarthquakeEvent()
    {
		Debug.Log ("Earthquake Full");
        state = EarthquakeState.MainEarthquake;
        // start sound loop TODO
        // violently shake screen TODO
		Camera.main.GetComponent<CameraShake>().ShakeFull();

        timeRemainingMainEarthquake = durationMainEarthquakeCurrent;
        durationMainEarthquakeCurrent += durationMainEarthquakeIncrement; // increase duration of the next earthquake
    }

    void EndEarthquakeEvent()
    {
		Debug.Log ("Earthquake Off");
        state = EarthquakeState.Inactive;
        // end sound loop TODO
		Camera.main.GetComponent<CameraShake>().ShakeOff();

        timeRemainingInactive = durationInactive;
    }
}

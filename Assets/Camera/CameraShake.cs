using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

    //private Quaternion defaultRotation;
	
	Vector3 originPosition;
	Vector3 positionOffset;
	
	private bool isShaking = false;
	
	private float warmupIntensity = 0.015f;
	private float fullIntensity = 0.2f;
	
	private float shake_intensity = 0.02f;
	private float shake_decay = 0.01f;
	
	private bool off = false;

	// Use this for initialization
	void Start () {
		originPosition = transform.position;
	}
	
	public void ShakeWarmup() {
		isShaking = true;
		shake_intensity = warmupIntensity;
		positionOffset = Vector3.zero;
		
		//rotationOffset = Vector4.zero;
		//shake_intensity = intensity;
		//shake_decay = decay;
	}
	
	public void ShakeFull() {
		isShaking = true;
		shake_intensity = fullIntensity;
		positionOffset = Vector3.zero;
	}
	
	public void ShakeOff() {
		off = true;
	}
	
	void ActualOff() {
		isShaking = false;
		transform.position = originPosition;
		positionOffset = Vector3.zero;
		off = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(isShaking) HandleShake();
	}
	
	void HandleShake() {
		positionOffset = Random.insideUnitSphere * shake_intensity;
		
		transform.position = originPosition + Random.insideUnitSphere * shake_intensity;
		
		if(off) shake_intensity -= shake_decay;
		
		if(shake_intensity <= 0) ActualOff();
	}
	

}




/*
function Initialize() {
	defaultRotation = transform.rotation;
}

function ShakeHeavy() {
	StartShake(intensities[0], decays[0]);
}
function ShakeMedium() {
	StartShake(intensities[1], decays[1]);
}
function ShakeLight() {
	StartShake(intensities[2], decays[2]);
}

function StartShake(intensity, decay) {
	//StartCoroutine(ShakeCoroutine());
	isShaking = true;
	positionOffset = Vector3.zero;
	rotationOffset = Vector4.zero;
	shake_intensity = intensity;
	shake_decay = decay;
}

function ShakeOff() {
	isShaking = false;
	positionOffset = Vector3.zero;
	rotationOffset = Vector4.zero;
}

function Update() {
	if(isShaking) HandleShake();
}

function HandleShake() {
	positionOffset = Random.insideUnitSphere * shake_intensity;
	
	rotationOffset.x = Random.Range(-shake_intensity,shake_intensity)*.2;
	rotationOffset.y = Random.Range(-shake_intensity,shake_intensity)*.2;
	rotationOffset.z = Random.Range(-shake_intensity,shake_intensity)*.2;
	rotationOffset.w = Random.Range(-shake_intensity,shake_intensity)*.2;
	
	shake_intensity -= shake_decay;
	
	if(shake_intensity <= 0) ShakeOff();
}*/
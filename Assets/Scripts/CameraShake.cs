using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {
	Vector3 originalCameraPosition;
	
	float shakeMagnitude = 0f;
	
	public Camera mainCamera;

	void Start() {
		originalCameraPosition = Camera.main.transform.position;
	}
	
	public void Shake(float magnitude, float duration) {
		shakeMagnitude = magnitude;
		InvokeRepeating("ShakeCamera", 0, .01f);
		Invoke("StopShaking", duration);
	}
	
	void ShakeCamera() {
		if(shakeMagnitude > 0) {
			float quakeX = Random.value * shakeMagnitude * 2 - shakeMagnitude;
			float quakeY = Random.value * shakeMagnitude * 2 - shakeMagnitude;
			Vector3 pos = Camera.main.transform.position;
			pos.x += quakeX;
			pos.y += quakeY;
			Camera.main.transform.position = pos;
		}
	}
	
	void StopShaking() {
		CancelInvoke("ShakeCamera");
		Camera.main.transform.position = originalCameraPosition;
	}
}

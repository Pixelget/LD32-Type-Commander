using UnityEngine;
using System.Collections;

public class CloudManager : MonoBehaviour {

	Vector3 tempPosition;
	Vector3 tempScale;
	float speed = 1.0f;

	float delay = 0f;

	void Start () {
		//delay = Random.Range(0f, Random.Range(5f,25f));
		ResetCloud ();

		// Set the star point
		tempPosition = transform.position;
		tempPosition.x = Random.Range(-12f, 12f);
		tempPosition.y = Random.Range(3.5f, 5f);
		transform.position = tempPosition;
	}

	void Update () {
		if (delay < 0f) {
			Move();
			CheckBoundary();
		} else {
			delay -= Time.deltaTime;
		}
	}

	void Move() {
		// Move to the right
		tempPosition = transform.position;
		tempPosition.x += (speed * Time.deltaTime);
		transform.position = tempPosition;
	}

	void CheckBoundary() {
		if (transform.position.x > 12f) {
			ResetCloud();
		}
	}

	void ResetCloud() {
		// Set the star point
		tempPosition = transform.position;
		tempPosition.x = -14f + Random.Range(-1.5f, 1.5f);
		tempPosition.y = Random.Range(3.5f, 5f);
		transform.position = tempPosition;

		// Change the scale is needed
		tempScale = transform.localScale;
		float scaleAdjust = Random.Range(0.5f, 1.2f);
		tempScale.x = scaleAdjust;
		tempScale.y = scaleAdjust;
		tempScale.z = scaleAdjust;
		transform.localScale = tempScale;

		// Change the speed
		speed = Random.Range(0.35f, 1.0f);
	}
}

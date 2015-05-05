using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyMove : MonoBehaviour {
	public Ability weakness = new Ability("Fire", 1);
	public float speed = 1f;
	Vector3 tempPosition = Vector3.zero;
	Vector3 textOffset = new Vector3(0f, 1f, 0f);
	GameObject weaknessTextObject;
	Text weaknessText;
	public GameObject deadObj;
	public GameObject smokeObj;

	void Start() {
		weaknessTextObject = GetWeaknessGameObject();
		if (weaknessTextObject != null) {
			weaknessText = weaknessTextObject.GetComponent<Text> ();
			weaknessText.text = weakness.name;
		}
	}

	void Update () {
		tempPosition = gameObject.transform.position;
		tempPosition.x -= speed * Time.deltaTime;
		gameObject.transform.position = tempPosition;
		
		if (weaknessTextObject != null) {
			weaknessTextObject.transform.position = tempPosition + textOffset;
		}

		if (transform.position.x <= -7.5f) {
			//Debug.Log("Terror Level Increase");
			GameObject.FindGameObjectWithTag("TerrorTracker").GetComponent<TerrorManager>().terrorLevel++;

			string cryOfTerror = "Oh god why?!";
			switch (Random.Range(0, 9)) {
			case 0:
				cryOfTerror = "Oh god why?!";
				break;
			case 1:
				cryOfTerror = "Help me!";
				break;
			case 2:
				cryOfTerror = "Not my baby!";
				break;
			case 3:
				cryOfTerror = "Why me!";
				break;
			case 4:
				cryOfTerror = "It hurts so much!";
				break;
			case 5:
				cryOfTerror = "We're finished!";
				break;
			case 6:
				cryOfTerror = "Run for your lives!";
				break;
			case 7:
				cryOfTerror = "The pain!";
				break;
			default:
				cryOfTerror = "Noooooooo!";
				break;
			}

			GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>().VillagerBubble(cryOfTerror, 5f);
			GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>().FlashRed();

			// Spawn a smoke particle effect on removal
			
			//GameObject temp = Instantiate (smokeObj);
			//temp.transform.position = new Vector3(transform.position.x, transform.position.y - 0.25f, 0f);

			Camera.main.GetComponent<CameraShake>().Shake (0.025f, 0.3f);

			weaknessTextObject.transform.position = new Vector3(12f, 0f, 0f);
			weaknessTextObject.SetActive(false);
			Destroy (gameObject);
		}
	}

	public void Kill() {
		// disable the enemytextbox and set it to null
		// play death anim
		// leave permanence
		// destroy object
		weaknessTextObject.transform.position = new Vector3(12f, 0f, 0f);
		weaknessTextObject.SetActive(false);

		if (deadObj != null) {
			GameObject temp = Instantiate (deadObj);
			temp.transform.position = new Vector3 (transform.position.x, transform.position.y - 0.25f, 0f);
		}

		// Remove an instance of the Ability on this enemy from the GameManager
		//GameManager.Instance.WeaknessList.Remove(weakness);
		Destroy (gameObject);
	}

	GameObject GetWeaknessGameObject() {
		// get a disabled game object with the tag WeaknessTextObject
		foreach (GameObject go in GameManager.Instance.weaknessTextObjectList) {
			if (!go.activeSelf) {
				go.SetActive(true);
				return go;
			}
		}
		return null;
	}
}
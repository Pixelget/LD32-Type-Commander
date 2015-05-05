using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class PlayerInput : MonoBehaviour {

	public GameObject playerTextObj;
	Text playerText;

	string playerCommand = "";
	List<Ability> EnemyWeaknessList = new List<Ability>();

	string playerInput = "";
	int currentLocationInCommand = 0;
	bool successfulInput = false;

	List<GameObject> enemyList = new List<GameObject>();
	GameObject target;

	public GameObject spr_OrderTextArea;
	public GameObject orderTextObject;
	Text orderText;

	public GameObject victoryImage;
	public GameObject defeatImage;

	bool finaleTriggered = false;

	public GameObject screenPanel;
	float flashTimer = 0f;
	float totalFlashTime = 0.15f;
	Color screenFlashColor;
	bool flashScreen = false;

	public GameObject SoundFXObj;
	AudioSource soundFX;

	void Start () {
		playerText = playerTextObj.GetComponent<Text>();

		orderText = orderTextObject.GetComponent<Text>();
		HideOrders();

		soundFX = SoundFXObj.GetComponent<AudioSource>();

		victoryImage.SetActive(false);
		defeatImage.SetActive(false);
	}

	void Update () {
		if (GameManager.Instance.victoryStatus == 0) {
			playerInput = Input.inputString;
			if (playerInput.Length > 0) { // The player typed something
				foreach (char c in playerInput) {
					successfulInput = false;
					currentLocationInCommand = playerCommand.Length;

					foreach (Ability command in GameManager.Instance.WeaknessList) {
						if ((command.name.Length > currentLocationInCommand) && (command.name.Substring (0, playerCommand.Length).ToLower () == playerCommand.ToLower ())) {
							if ((c.ToString ().ToLower () == command.name [currentLocationInCommand].ToString ().ToLower ()) && (!successfulInput)) {
								playerCommand += c;
								successfulInput = true;
								UpdatePlayerText ();
							}
						}
					}

					if (!successfulInput) {
						ResetPlayerCommand ();
					}
				}
			}
		} else if (GameManager.Instance.victoryStatus > 0) {
			if (!finaleTriggered) {
				if (GameManager.Instance.victoryStatus == 1) {
					// Defeat
					defeatImage.SetActive(true);
					VillagerBubble("Oh My God I'm Burning!", 5f);
					finaleTriggered = true;
				} else {
					// Victory
					victoryImage.SetActive(true);
					VillagerBubble("Thank you Hero!", 5f);
					finaleTriggered = true;
				}
			}
		}

		if (flashScreen) {
			flashScreenUpdate ();
		}
	}

	void UpdatePlayerText() {
		playerText.text = playerCommand;
		CheckForSuccessfulCommand();
	}

	void CheckForSuccessfulCommand() {
		bool useAbility = false;
		Ability temp = new Ability("", 0);
		foreach (Ability command in GameManager.Instance.WeaknessList) {
			if (command.name.ToLower() == playerCommand.ToLower()) {
				useAbility = true;
				temp = command;
			}
		}
		if (useAbility) {
			UseAbility(temp);
		}
	}
	
	void ResetPlayerCommand() {
		playerCommand = "";
		UpdatePlayerText();
		// Shake here / show the error of the players ways
	}
	
	void ClearPlayerCommand() {
		playerCommand = "";
		StartCoroutine(ClearPlayerCommandCR(0.2f));
	}

	IEnumerator ClearPlayerCommandCR(float t) {
		for (float timer = t; timer >= 0; timer -= Time.deltaTime)
			yield return 0;
		
		UpdatePlayerText();
	}

	void UseAbility(Ability command) {
		// Play the using ability animations (switch on the command?)
		// Play ability sound
		enemyList = GameObject.FindGameObjectsWithTag("Enemy").ToList();
		target = GetClosestEnemy(enemyList, command);

		if (target != null) {
			soundFX.Play();

			target.SendMessage("Kill", SendMessageOptions.DontRequireReceiver);

			Camera.main.GetComponent<CameraShake>().Shake (0.02f, 0.15f);
			ShowOrders("Mages use " + command.name + "!");
			//StartCoroutine(ClearPlayerOrderCR(1.5f));
			CancelInvoke("HideOrders");
			Invoke("HideOrders", 0.5f);

			switch(command.typeid) {
			case 0:
				FlashWhite();
				break;
			case 1:
				FlashRed();
				break;
			case 2:
				FlashBlue();
				break;
			}
		}

		ClearPlayerCommand();
	}

	public void VillagerBubble(string cryofterror, float delay) {
		ShowOrders(cryofterror);
		Invoke("HideOrders", delay);
	}

	GameObject GetClosestEnemy(List<GameObject> elist, Ability command) {
		// get the enemy that is to the right of the character and the closest
		if (elist.Count > 0) {
			// get the closest enemy that has the command as a weakness
			float distance = 1000f;
			float tempDistance;
			GameObject current = null;

			// loop through the elist
			foreach (GameObject enemyGO in elist) {
				tempDistance = (gameObject.transform.position - enemyGO.transform.position).magnitude;
				if ((tempDistance < distance) && (enemyGO.GetComponent<EnemyMove>().weakness.name.ToString().ToLower() == command.name.ToLower())) {
					current = enemyGO;
					distance = tempDistance;
				}
			}

			return current;
		}
		return null;
	}

	void ShowOrders(string text) {
		orderTextObject.SetActive(true);
		spr_OrderTextArea.SetActive(true);
		orderText.text = text;
	}

	void HideOrders() {
		orderTextObject.SetActive(false);
		spr_OrderTextArea.SetActive(false);
	}
	
	void FlashWhite() {
		FlashScreenWhite();
		Invoke("FlashScreenWhite", 0.1f);
	}
	public void FlashRed() {
		FlashScreenRed();
		Invoke("FlashScreenRed", 0.1f);
	}
	void FlashBlue() {
		FlashScreenBlue();
		Invoke("FlashScreenBlue", 0.1f);
	}

	void FlashScreenWhite() {
		flashScreen = true;
		flashTimer = 0f;
		screenFlashColor = new Color(1f,1f,1f,0.8f);
	}
	void FlashScreenBlue() {
		flashScreen = true;
		flashTimer = 0f;
		screenFlashColor = new Color(0.2f,0.2f,0.85f,0.8f);
	}
	void FlashScreenRed() {
		flashScreen = true;
		flashTimer = 0f;
		screenFlashColor = new Color(0.85f,0.2f,0.2f,0.8f);
	}

	void flashScreenUpdate() {
		flashTimer += Time.deltaTime;
		if (flashTimer >= totalFlashTime) {
			flashTimer = totalFlashTime;
			flashScreen = false;
		}

		screenFlashColor.a = 0.8f * (1f - (flashTimer / totalFlashTime));
		screenPanel.GetComponent<Image>().color = screenFlashColor;
	}
}

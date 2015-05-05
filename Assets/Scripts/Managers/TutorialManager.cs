using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialManager : MonoBehaviour {

	public GameObject textDisplay;
	public Text textForTutorial;

	public GameObject TutorialMinion;
	Vector3 spawnLocation = new Vector3(10f, -1f, 0f);

	public GameObject GameLogo;

	int terrorCheck = 0;

	enum TutorialStep {
		startScreen,
		talk0,
		talk1,
		talk2,
		talk3,
		Minion,
		Check,
		Finishing
	}

	TutorialStep tutStep = TutorialStep.startScreen;

	void Update () {
		switch (tutStep) {
		case TutorialStep.startScreen:
			textDisplay.SetActive(false);
			textForTutorial.gameObject.SetActive(false);

			// Display the Press any Key and Logo
			
			if (Input.anyKeyDown) {
				GameLogo.SetActive(false);
				tutStep = TutorialStep.talk0;
			}
			
			break;
		case TutorialStep.talk0:
			textDisplay.SetActive(true);
			textForTutorial.gameObject.SetActive(true);
			textForTutorial.text = "Welcome Typing Hero, we are currently in a bad spot and you came at the right moment, please help us. Hit any button to advance the text.";
			
			if (Input.anyKeyDown) {
				tutStep = TutorialStep.talk1;
			}
			
			break;
		case TutorialStep.talk1:
			textForTutorial.text = "To kill enemies you need to type your orders. Want to check what you are typing? look to the lower left corner.";
			
			if (Input.anyKeyDown) {
				tutStep = TutorialStep.talk2;
			}
			
			break;
		case TutorialStep.talk2:
			textForTutorial.text = "Not sure what you need to type, no problem. Watch the enemy closely, they are practically telling you what they are weak to, you just need to type what you see.";
			
			if (Input.anyKeyDown) {
				tutStep = TutorialStep.talk3;
			}
			break;
		case TutorialStep.talk3:
			textForTutorial.text = "Here comes a single enemy now, use your typing skills to practice telling your soldiers what to do.";
			
			if (Input.anyKeyDown) {
				textDisplay.SetActive(false);
				textForTutorial.gameObject.SetActive(false);
				tutStep = TutorialStep.Minion;
			}
			break;
		case TutorialStep.Minion:
			// Spawn a minion for the player to kill
			SpawnMinion();
			tutStep = TutorialStep.Check;
			GameManager.Instance.victoryStatus = 0;
			break;
		case TutorialStep.Check:
			if (GameObject.FindGameObjectsWithTag("Enemy").Length < 1) {
				// No more enemy
				if (GameObject.FindGameObjectWithTag("TerrorTracker").GetComponent<TerrorManager>().terrorLevel > terrorCheck) {
					// Player let the minion get through
					terrorCheck++;
					tutStep = TutorialStep.Minion;
					Debug.Log("Player Failed To Kill the minion");
				} else {
					// Player completed the tutorial
					Debug.Log("Completed the tutorial");
					tutStep = TutorialStep.Finishing;
				}
			}
			break;
		case TutorialStep.Finishing:
			textDisplay.SetActive(true);
			textForTutorial.gameObject.SetActive(true);
			textForTutorial.text = "Good job, but don't slack off now, there are many more monsters for you to fight.";

			// show Congrats screen
			if (Input.anyKeyDown) {
				textDisplay.SetActive(false);
				textForTutorial.gameObject.SetActive(false);

				GameManager.Instance.WeaknessList.Clear();
				GameObject.FindGameObjectWithTag("WaveManager").GetComponent<WaveManager>().SpawnEnabled = true;
				gameObject.SetActive(false);
			}
			break;
		}
	}

	void SpawnMinion() {
		Debug.Log("Spawning Tutorial Minion");
		Ability weakness = new Ability("", 0);
		switch (Random.Range(0, 3)) {
		case 0:
			weakness = new Ability("Fire", 1);
			break;
		case 1:
			weakness = new Ability("Flame", 1);
			break;
		case 2:
			weakness = new Ability("Burn", 1);
			break;
		default:
			Debug.Log("Ability Out of range in Slime");
			break;
		}
		
		GameObject temp = (GameObject) Instantiate(TutorialMinion);
		temp.transform.position = spawnLocation;
		temp.GetComponent<EnemyMove>().weakness = weakness;
		if (!GameManager.Instance.hasWeaknessInList(weakness)) {
			GameManager.Instance.WeaknessList.Add(weakness);
		}
	}
}

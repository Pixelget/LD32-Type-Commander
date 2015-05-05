using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TerrorManager : MonoBehaviour {

	public List<Image> TerrorTracker = new List<Image> ();

	public int terrorLevel = 0;

	void Update() {
		updateTerror();
	}

	void updateTerror() {
		for (int i = 0; i < terrorLevel; i++) {
			if (i < 5) {
				Color tempC = TerrorTracker[i].color;
				tempC.a = 1.0f;
				TerrorTracker[i].color = tempC;
			}
		}

		CheckGameOver();
	}

	void CheckGameOver() {
		if (terrorLevel >= 5) {
			//Start the end of game sequence
			//Debug.Log("Game Over - You Lose!");
			GameManager.Instance.victoryStatus = 1;
		}
	}
}

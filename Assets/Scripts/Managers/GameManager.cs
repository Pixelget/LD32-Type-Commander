using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour {
	public List<GameObject> weaknessTextObjectList;
	public List<Ability> WeaknessList;

	public float difficultySetting = 0.5f;

	public int victoryStatus = -1; // -1 = Tutorial, 0 = Not Determined, 1 = Defeat, 2 = Victory

	static GameManager _instance;
	static public bool isActive { 
		get { 
			return _instance != null; 
		} 
	}
	
	static public GameManager Instance {
		get {
			if (_instance == null) {
				_instance = Object.FindObjectOfType(typeof(GameManager)) as GameManager;
				
				if (_instance == null) {
					GameObject go = new GameObject("_GameManager");
					DontDestroyOnLoad(go);
					_instance = go.AddComponent<GameManager>();
					Debug.Log("Debug: Creating A New Game Manager");
				}
				
				_instance.InitializeGameVariables();
			}
			return _instance;
		}
	}
	
	public void OnApplicationQuit() {
		_instance = null;
	}
	
	public void LoadState(string scene) {
		Application.LoadLevel(scene);
	}

	private void InitializeGameVariables() {
		// Create the Enemy List of spawnable enemies

		// Get a list of the weaknessTextGO
		WeaknessList = new List<Ability>();
		weaknessTextObjectList = new List<GameObject>();
		weaknessTextObjectList = GameObject.FindGameObjectsWithTag("WeaknessTextObject").ToList();
		foreach (GameObject go in weaknessTextObjectList) {
			go.SetActive(false);
		}
	}

	public bool hasWeaknessInList(Ability a) {
		// loop through the list and see if the weakness exists
		return false;
	}
}

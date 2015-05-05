using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour {
	// Game Information
	int totalWaves = 10;
	
	List<Vector3> groundSpawnPoints = new List<Vector3>() {
		new Vector3(10f, 1f, 0f),
		new Vector3(10f, 0f, 0f),
		new Vector3(10f, -1f, 0f),
		new Vector3(10f, -2f, 0f),
		new Vector3(10f, -3f, 0f)
	};
	List<Vector3> airSpawnPoints = new List<Vector3>() {
		new Vector3(10f, 1f, 0f),
		new Vector3(10f, 2f, 0f),
		new Vector3(10f, 3f, 0f)
	};

	// Wave Information
	public int currentWave = 0;
	bool spawnWave = false;
	bool DoneSpawningWave = false;
	bool cooldownBeforeNextWave = true;
	public bool SpawnEnabled = false;
	float cooldown = 3f;

	// Wave UI
	public Text waveText;
	
	// Enemy Types and associated abilities
	public GameObject NormalMinion; // Slime
	public GameObject FastMinion;
	public GameObject AirMinion;
	public GameObject HeavyMinion;
	public GameObject Boss;

	public enum MonsterType {
		NormalMinion,
		FastMinion,
		AirMinion,
		HeavyMinion,
		Boss
	}

	// Update is called once per frame
	void Update () {
		if (SpawnEnabled) {
			if ((spawnWave) && (GameObject.FindGameObjectsWithTag ("Enemy").Length <= 0)) {
				// Create a wave
				GameManager.Instance.WeaknessList.Clear();
				SpawnWave ();
				spawnWave = false;
			}

			if ((DoneSpawningWave) && (GameObject.FindGameObjectsWithTag ("Enemy").Length <= 0)) {
				DoneSpawningWave = false;
				cooldownBeforeNextWave = true;
				cooldown = 3f;
			}

			if (cooldownBeforeNextWave) {
				cooldown -= Time.deltaTime;

				if (cooldown < 0f) {
					spawnWave = true;
					cooldownBeforeNextWave = false;
					cooldown = 3f;
				}
			}
		}
		
		if (currentWave > 10) {
			waveText.text = "You Win!";
		} else if(currentWave == 0) {
			waveText.text = "Tutorial";
		}else {
			waveText.text = "Wave " + string.Format ("{0:00}", currentWave);
		}
	}

	/// <summary>
	/// Spawns the minion with a start delay.
	/// </summary>
	/// <param name="numberOfMinions">Number of minions to spawn.</param>
	/// <param name="timeBetween">Time between spawns.</param>
	/// <param name="delay">Delay Time before spawn starts.</param>
	/// <param name="monster">Monster Type to spawn.</param>
	IEnumerator SpawnMinion(int numberOfMinions, float timeBetween, float delay, MonsterType monster) {
		for (float timer = delay; timer >= 0; timer -= Time.deltaTime)
			yield return 0;

		StartCoroutine(SpawnMinion(numberOfMinions, timeBetween, monster));
	}

	IEnumerator SpawnMinion(int n, float t, MonsterType m) {
		for (int i = 0; i < n; i++) {
			for (float timer = t; timer >= 0; timer -= Time.deltaTime)
				yield return 0;

			switch(m) {
			case MonsterType.NormalMinion:
				SpawnNormalMinion();
				break;
			case MonsterType.FastMinion:
				SpawnFastMinion();
				break;
			case MonsterType.AirMinion:
				SpawnAirMinion();
				break;
			case MonsterType.HeavyMinion:
				SpawnHeavyMinion();
				break;
			case MonsterType.Boss:
				SpawnBoss();
				break;
			}
		}

		DoneSpawningWave = true;
	}

	void SpawnNormalMinion() {
		Ability weakness = new Ability("", 0);
		switch (Random.Range(0, 8)) {
		case 0:
			weakness = new Ability("Fire", 1);
			break;
		case 1:
			weakness = new Ability("Flame", 1);
			break;
		case 2:
			weakness = new Ability("Burn", 1);
			break;
		case 3:
			weakness = new Ability("Ember", 1);
			break;
		case 4:
			weakness = new Ability("Heat", 1);
			break;
		case 5:
			weakness = new Ability("Flare", 1);
			break;
		case 6:
			weakness = new Ability("Pyre", 1);
			break;
		case 7:
			weakness = new Ability("Searing", 1);
			break;
		default:
			Debug.Log("Ability Out of range in Slime");
			break;
		}

		GameObject temp = (GameObject) Instantiate(NormalMinion);
		Vector3 tempspawn = groundSpawnPoints[Random.Range (0, 4)];
		temp.transform.position = new Vector3(tempspawn.x, tempspawn.y + Random.Range(-0.8f, 0.5f), tempspawn.z);
		temp.GetComponent<EnemyMove>().weakness = weakness;
		if (!GameManager.Instance.hasWeaknessInList(weakness)) {
			GameManager.Instance.WeaknessList.Add(weakness);
		}
	}
	
	void SpawnFastMinion() {
		Ability weakness = new Ability("", 0);
		switch (Random.Range(0, 5)) {
		case 0:
			weakness = new Ability("Ice", 2);
			break;
		case 1:
			weakness = new Ability("Frost", 2);
			break;
		case 2:
			weakness = new Ability("Freeze", 2);
			break;
		case 3:
			weakness = new Ability("Hail", 2);
			break;
		case 4:
			weakness = new Ability("Glacier", 2);
			break;
		default:
			Debug.Log("Ability Out of range in Slime");
			break;
		}
		
		GameObject temp = (GameObject) Instantiate(FastMinion);
		Vector3 tempspawn = groundSpawnPoints[Random.Range (0, 4)];
		temp.transform.position = new Vector3(tempspawn.x, tempspawn.y + Random.Range(-0.8f, 0.5f), tempspawn.z);
		temp.GetComponent<EnemyMove>().weakness = weakness;
		temp.GetComponent<EnemyMove>().speed = 2.5f;
		if (!GameManager.Instance.hasWeaknessInList(weakness)) {
			GameManager.Instance.WeaknessList.Add(weakness);
		}
	}
	
	void SpawnAirMinion() {
		Ability weakness = new Ability("", 0);
		switch (Random.Range(0, 6)) {
		case 0:
			weakness = new Ability("Thunder", 0);
			break;
		case 1:
			weakness = new Ability("Lightning", 0);
			break;
		case 2:
			weakness = new Ability("Shock", 0);
			break;
		case 3:
			weakness = new Ability("Spark", 0);
			break;
		case 4:
			weakness = new Ability("Crackle", 0);
			break;
		case 5:
			weakness = new Ability("Bolt", 0);
			break;
		default:
			Debug.Log("Ability Out of range in Slime");
			break;
		}
		
		GameObject temp = (GameObject) Instantiate(AirMinion);
		Vector3 tempspawn = airSpawnPoints[Random.Range (0, 3)];
		temp.transform.position = new Vector3(tempspawn.x, tempspawn.y + Random.Range(-0.8f, 0.5f), tempspawn.z);
		temp.GetComponent<EnemyMove>().weakness = weakness;
		temp.GetComponent<EnemyMove>().speed = 1.25f;
		if (!GameManager.Instance.hasWeaknessInList(weakness)) {
			GameManager.Instance.WeaknessList.Add(weakness);
		}
	}
	
	void SpawnHeavyMinion() {
		
	}
	
	void SpawnBoss() {
		
	}
	
	void SpawnWave() {
		// Spawn enemies based on waves
		switch (currentWave) {
		case 0:
			StartCoroutine(SpawnMinion(Mathf.FloorToInt(6 * GameManager.Instance.difficultySetting), Random.Range(0.5f, 0.85f), 0f, MonsterType.NormalMinion));
			break;
		case 1:
			StartCoroutine(SpawnMinion(Mathf.FloorToInt(10 * GameManager.Instance.difficultySetting), Random.Range(0.5f, 0.85f), 0f, MonsterType.NormalMinion));
			break;
		case 2:
			StartCoroutine(SpawnMinion(Mathf.CeilToInt(8 * GameManager.Instance.difficultySetting), Random.Range(0.5f, 0.85f), 0f, MonsterType.NormalMinion));
			StartCoroutine(SpawnMinion(Mathf.CeilToInt(4 * GameManager.Instance.difficultySetting), Random.Range(0.75f, 1.15f), 0.5f, MonsterType.FastMinion));
			break;
		case 3:
			StartCoroutine(SpawnMinion(Mathf.CeilToInt(12 * GameManager.Instance.difficultySetting), Random.Range(0.5f, 0.85f), 0f, MonsterType.NormalMinion));
			StartCoroutine(SpawnMinion(Mathf.CeilToInt(4 * GameManager.Instance.difficultySetting), Random.Range(0.75f, 1.15f), 0.5f, MonsterType.FastMinion));
			break;
		case 4:
			StartCoroutine(SpawnMinion(Mathf.CeilToInt(8 * GameManager.Instance.difficultySetting), Random.Range(0.5f, 0.85f), 0f, MonsterType.NormalMinion));
			StartCoroutine(SpawnMinion(Mathf.CeilToInt(6 * GameManager.Instance.difficultySetting), Random.Range(0.75f, 1.15f), 0.5f, MonsterType.FastMinion));
			StartCoroutine(SpawnMinion(Mathf.CeilToInt(4 * GameManager.Instance.difficultySetting), Random.Range(0.25f, 0.5f), 0.25f, MonsterType.AirMinion));
			break;
		case 5:
			StartCoroutine(SpawnMinion(Mathf.CeilToInt(12 * GameManager.Instance.difficultySetting), Random.Range(0.5f, 0.85f), 0f, MonsterType.NormalMinion));
			StartCoroutine(SpawnMinion(Mathf.CeilToInt(8 * GameManager.Instance.difficultySetting), Random.Range(0.25f, 0.5f), 0.25f, MonsterType.AirMinion));
			break;
		case 6:
			StartCoroutine(SpawnMinion(Mathf.CeilToInt(16 * GameManager.Instance.difficultySetting), Random.Range(0.5f, 0.85f), 0f, MonsterType.NormalMinion));
			StartCoroutine(SpawnMinion(Mathf.CeilToInt(4 * GameManager.Instance.difficultySetting), Random.Range(0.75f, 1.15f), 0.5f, MonsterType.FastMinion));
			StartCoroutine(SpawnMinion(Mathf.CeilToInt(8 * GameManager.Instance.difficultySetting), Random.Range(0.25f, 0.5f), 0.25f, MonsterType.AirMinion));
			break;
		case 7:
			StartCoroutine(SpawnMinion(Mathf.CeilToInt(16 * GameManager.Instance.difficultySetting), Random.Range(0.5f, 0.85f), 0f, MonsterType.NormalMinion));
			StartCoroutine(SpawnMinion(Mathf.CeilToInt(4 * GameManager.Instance.difficultySetting), Random.Range(0.75f, 1.15f), 0.5f, MonsterType.FastMinion));
			StartCoroutine(SpawnMinion(Mathf.CeilToInt(14 * GameManager.Instance.difficultySetting), Random.Range(0.25f, 0.5f), 0.25f, MonsterType.AirMinion));
			StartCoroutine(SpawnMinion(Mathf.CeilToInt(2 * GameManager.Instance.difficultySetting), Random.Range(1f, 2f), 2.5f, MonsterType.HeavyMinion));
			break;
		case 8:
			StartCoroutine(SpawnMinion(Mathf.CeilToInt(16 * GameManager.Instance.difficultySetting), Random.Range(0.5f, 0.85f), 0f, MonsterType.NormalMinion));
			StartCoroutine(SpawnMinion(Mathf.CeilToInt(4 * GameManager.Instance.difficultySetting), Random.Range(0.75f, 1.15f), 0.5f, MonsterType.FastMinion));
			StartCoroutine(SpawnMinion(Mathf.CeilToInt(8 * GameManager.Instance.difficultySetting), Random.Range(0.25f, 0.5f), 0.25f, MonsterType.AirMinion));
			StartCoroutine(SpawnMinion(Mathf.CeilToInt(4 * GameManager.Instance.difficultySetting), Random.Range(1f, 2f), 2.5f, MonsterType.HeavyMinion));
			break;
		case 9:
			StartCoroutine(SpawnMinion(Mathf.CeilToInt(30 * GameManager.Instance.difficultySetting), Random.Range(0.5f, 0.85f), 0f, MonsterType.NormalMinion));
			StartCoroutine(SpawnMinion(Mathf.CeilToInt(10 * GameManager.Instance.difficultySetting), Random.Range(0.75f, 1.15f), 0.5f, MonsterType.FastMinion));
			StartCoroutine(SpawnMinion(Mathf.CeilToInt(8 * GameManager.Instance.difficultySetting), Random.Range(0.25f, 0.5f), 0.25f, MonsterType.AirMinion));
			StartCoroutine(SpawnMinion(Mathf.CeilToInt(6 * GameManager.Instance.difficultySetting), Random.Range(1f, 2f), 2.5f, MonsterType.HeavyMinion));
			StartCoroutine(SpawnMinion(Mathf.CeilToInt(1 * GameManager.Instance.difficultySetting), Random.Range(0.15f, 0.35f), 5f, MonsterType.Boss));
			break;
		case 10:
			Debug.Log("Game Over - You Win!");
			if (GameManager.Instance.victoryStatus != 1) {
				GameManager.Instance.victoryStatus = 2;
			}
			break;
		}
		
		if (GameManager.Instance.victoryStatus == 0) {
			currentWave++;
		}
	}
}

using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public GameObject[] hazards; //make this an array, and then randomize to spawn random asteroids.
	public Vector3 spawnValues;
	private int hazardCount;
	public int minHazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;
	public int hazardIncrease;
	private int waveCount;

	private float timeSinceCrash;

	public GUIText scoreText;
	private int score;
	public GUIText restartText;
	public GUIText gameOverText;
	private bool gameOver;
	private bool restart;
	public GUIText energyText;
	void Start (){
		score = 0;
		UpdateScore ();
		StartCoroutine (SpawnWaves ());
		gameOver = false;
		restart = false;
		restartText.text = "";
		gameOverText.text = "";
		waveCount = 0;
		hazardCount = minHazardCount;
	}
	void Update (){
		if (restart) {
			if (Input.GetKeyDown (KeyCode.R)) {
				Application.LoadLevel (Application.loadedLevel); //use this to load different scenes too!
			}
		}
	}
	IEnumerator SpawnWaves (){
		yield return new WaitForSeconds (startWait);
		while(true){
			for (int i=0;i<hazardCount;i++) {
				GameObject hazard = hazards[(Random.Range(0,hazards.Length))];
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds (spawnWait);
			}
			if(gameOver){
				restartText.text = "Press R to restart";
				restart = true;
				break;
			}
			waveCount ++;
			hazardCount = minHazardCount + waveCount*hazardIncrease;
			yield return new WaitForSeconds (waveWait);
		}
	}
	void UpdateScore (){
		scoreText.text = "Score: " + score; 
	}
	public void AddScore (int newScoreValue){
		score = score +newScoreValue;
		UpdateScore ();
	}
	public void GameOver (){
		gameOverText.text = "Game Over!";
		gameOver = true;
	}
	public void UpdateEnergy (float energy){
		energyText.text = "Energy: " + energy;
	}
}

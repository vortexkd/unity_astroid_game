using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour {
	public GameObject explosion;
	public GameObject playerExplosion;
	private GameController gameController;
	public int scoreValue;
	private PlayerController player;
	void Start(){
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController>();
		}
		if (gameController == null) {
			Debug.Log ("Cannot find 'Game Controller' script");
		}
		GameObject playerControllerObject = GameObject.FindWithTag ("Player");
		if (playerControllerObject != null) {
			player = playerControllerObject.GetComponent<PlayerController> ();
		} 
		else {
			Debug.Log ("Cannot find 'Player' script");
		}
	}
	void OnTriggerEnter(Collider other){
		if (other.tag == "Boundary") {
			return;
		}
		Instantiate(explosion, transform.position, transform.rotation);
		if (other.tag == "Player") {
			Instantiate (playerExplosion, other.transform.position, other.transform.rotation);
			//gameController.GameOver();
			player.PlayerCrash();
		}
		gameController.AddScore (scoreValue);
		if (other.tag != "Player") {
			Destroy (other.gameObject);
		}
		Destroy (gameObject);
	}
}
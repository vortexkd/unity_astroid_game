using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary 
{
	public float xMin, xMax, zMin, zMax;
}


public class PlayerController : MonoBehaviour {

	public Rigidbody rb;
	public AudioSource audio;
	public float speed; // can we go more than 100 energy? if not, this should start high?
	public float minSpeed;
	public float netSpeed;
	public Boundary boundary;
	public float tilt;
	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
	private float nextFire;

	public float playerEnergy;
	public int boltEnergyCost;
	private float crashValue;
	private float lastCrash;
	public float scalingFactor;
	public int energyWait;

	GameController gameController;
	
	void Start(){
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent<GameController>();
		}
		if (gameController == null) {
			Debug.Log ("Cannot find 'Game Controller' script");
		}
		rb = GetComponent<Rigidbody>();
		audio = GetComponent<AudioSource>();
		StartCoroutine (HealthIncrease ());
		crashValue = Time.time;
		lastCrash = Time.time;
	}
	void Update (){
		if (Input.GetButton ("Fire1") && Time.time > nextFire)
		{
			nextFire = Time.time + fireRate;
			/*GameObject clone = */Instantiate(shot, shotSpawn.position, shotSpawn.rotation);// as GameObject;
			audio.Play();
			playerEnergy = playerEnergy - boltEnergyCost;
			gameController.UpdateEnergy(playerEnergy);
		}
	}
	void FixedUpdate (){
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		netSpeed = (minSpeed + (speed * (playerEnergy / 100)));
		Vector3 movement = new Vector3(moveHorizontal*netSpeed, 0.0f, moveVertical*netSpeed);
		rb.velocity = movement;
		rb.position = new Vector3 
			(
				Mathf.Clamp (rb.position.x, boundary.xMin+0.5f, boundary.xMax-0.5f),
				0.0f,
				Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
				);
		rb.rotation = Quaternion.Euler(0.0f,0.0f,rb.velocity.x* -tilt);
		crashValue = (Time.time - lastCrash) / (Time.time - lastCrash + (2 * scalingFactor));
	}

	IEnumerator HealthIncrease (){
		while (true) {
			if(playerEnergy < 100){
				playerEnergy = playerEnergy + 1;
			}
			gameController.UpdateEnergy (playerEnergy);
			yield return new WaitForSeconds (energyWait);
		}
	}
	public void PlayerCrash(){
		playerEnergy = playerEnergy - 20;
		//crashValue = (Time.time + lastCrash) / 2;
		lastCrash = Time.time;
		Debug.Log ("Crash Value = " + crashValue);
		gameController.UpdateEnergy (playerEnergy);
		if (playerEnergy < 0) {
			gameController.GameOver();
			Destroy(gameObject);
		}
	}
	public float getEnergy(){
		return playerEnergy;
	}
	public float getCrashValue(){
		return crashValue;
	}
}

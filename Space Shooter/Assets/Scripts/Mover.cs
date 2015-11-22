using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {

	public Rigidbody rb;
	public float speed;
	public float speedIncrease;
	private float timeSinceCrash; // well this isn't really the time since the last crash, but something like that.
	private PlayerController player;
	// Use this for initialization
	void Start () {
		GameObject playerControllerObject = GameObject.FindWithTag ("Player");
		if (playerControllerObject != null) {
			player = playerControllerObject.GetComponent<PlayerController>();
			timeSinceCrash = player.getCrashValue ();
		}
		if (playerControllerObject == null) {
			Debug.Log ("Cannot find 'Player Controller' script");
			timeSinceCrash = 0;
		}
		rb = GetComponent<Rigidbody>();
		//rb.velocity = transform.forward * (speed + (speedIncrease * (timeSinceCrash/(timeSinceCrash+scalingFactor))));
		rb.velocity = transform.forward * (speed + (speedIncrease*timeSinceCrash));
	}
}
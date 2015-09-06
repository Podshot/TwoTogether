using UnityEngine;
using System.Collections;

// Handles side collisions for Small Cube character
public class SmallCubeColliderScript : MonoBehaviour {

	public string side;

	private SpawnHandler handler;
	private Controller controller;

	void Awake() {
		controller = GetComponentInParent<Controller>();
	}

	void Start() {
		handler = GameObject.FindGameObjectWithTag("SpawnController").GetComponent<SpawnHandler>();
	}

	void OnCollisionEnter2D(Collision2D collided) {
		if (collided.transform.tag == "Killer_Red") {
			handler.ResetRedCube();
		} else {
			controller.SetCollided(side, true);
		}
	}

	void OnCollisionExit2D(Collision2D collided) {
		controller.SetCollided(side, false);
	}
}

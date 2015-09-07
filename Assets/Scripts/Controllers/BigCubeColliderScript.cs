using UnityEngine;
using System.Collections;

// Handles side collisions for Big Rectangle character
public class BigCubeColliderScript : MonoBehaviour {

	public string side;

	private SpawnHandler handler;
	private Controller controller;

	void Awake() {
		controller = GetComponentInParent<Controller>();
	}

	void Start() {
		handler = GameObject.FindGameObjectWithTag("SpawnController").GetComponent<SpawnHandler>();
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.tag != "Objective") {
			controller.SetCollided(side, true);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		controller.SetCollided(side, false);
	}
}

using UnityEngine;
using System.Collections;

// Handles side collisions for Small Cube character
public class SmallCubeColliderScript : MonoBehaviour {

	public string side;

	private SpawnHandler handler;
	private SmallCubeController controller;

	void Awake() {
		controller = GetComponentInParent<SmallCubeController>();
		handler = GameObject.FindGameObjectWithTag("SpawnController").GetComponent<SpawnHandler>();
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.transform.tag == "Killer_Red") {
			handler.ResetRedCube();
		} else {
			controller.SetCollided(side, true);
		}
	}
	
	void OnTriggerExit2D(Collider2D other) {
		controller.SetCollided(side, false);
	}
}

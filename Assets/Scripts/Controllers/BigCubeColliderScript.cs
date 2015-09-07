using UnityEngine;
using System.Collections;

// Handles side collisions for Big Rectangle character
public class BigCubeColliderScript : MonoBehaviour {

	public string side;

	private SpawnHandler handler;
	private BigCubeController controller;

	void Awake() {
		controller = GetComponentInParent<BigCubeController>();
		handler = GameObject.FindGameObjectWithTag("SpawnController").GetComponent<SpawnHandler>();
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.transform.tag == "Killer_Blue") {
			handler.ResetBlueCube();
		} else {
			controller.SetCollided(side, true);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		controller.SetCollided(side, false);
	}
}

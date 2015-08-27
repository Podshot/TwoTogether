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

	void OnCollisionEnter2D(Collision2D collided) {
		if (collided.transform.tag == "Killer_Blue") {
			handler.ResetBlueCube();
		} else {
			controller.SetCollided(side, true);
		}
	}

	void OnCollisionExit2D(Collision2D collided) {
		controller.SetCollided(side, false);
	}
}

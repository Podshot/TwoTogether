using UnityEngine;
using System.Collections;

public class SmallCubeColliderScript : MonoBehaviour {

	public string side;

	private SpawnHandler handler;
	private SmallCubeController controller;

	void Awake() {
		controller = GetComponentInParent<SmallCubeController>();
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

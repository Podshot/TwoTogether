using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class KillerTerrain : MonoBehaviour {

	[SerializeField] private string targetTag = "none";

	private SpawnHandler handler;

	void Awake() {
		handler = GameObject.FindGameObjectWithTag("SpawnController").GetComponent<SpawnHandler>();
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag.Equals(targetTag)) {
			if (targetTag.Equals("BigCube")) {
				handler.ResetBlueCube();
			} else if (targetTag.Equals("SmallCube")) {
				handler.ResetRedCube();
			} else {
				print(string.Format("This shouldn't ever display. TargetTag={0}", targetTag));
			}
		}
	}
}

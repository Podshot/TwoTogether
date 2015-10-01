using UnityEngine;
using System.Collections.Generic;

public class SpawnHandler : MonoBehaviour {

	public GameObject smallCubePrefab;
	public GameObject bigCubePrefab;
	
	private Transform smallCubeSpawn;
	private Transform bigCubeSpawn;
	private GameObject smallCubeActive;
	private GameObject bigCubeActive;

	public void Load() {
		smallCubeSpawn = transform.Find("SmallCubeSpawnpoint");
		bigCubeSpawn = transform.Find("BigCubeSpawnpoint");

		smallCubeActive = (GameObject) Instantiate(smallCubePrefab, smallCubeSpawn.position, Quaternion.identity);
		bigCubeActive = (GameObject) Instantiate(bigCubePrefab, bigCubeSpawn.position, Quaternion.identity);
	}

    public List<Controller> GetControllers() {
        List<Controller> list = new List<Controller>() { smallCubeActive.GetComponent<Controller>(), bigCubeActive.GetComponent<Controller>() };
        return list;
    }

    // Handles quitting and reset
	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}
		if (Input.GetKeyDown(KeyCode.R)) {
			ResetBlueCube();
			ResetRedCube();
		}
	}

	public void ResetRedCube() {
		smallCubeActive.transform.position = smallCubeSpawn.position;
	}

	public void ResetBlueCube() {
		bigCubeActive.transform.position = bigCubeSpawn.position;
	}
}

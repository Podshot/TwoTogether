using UnityEngine;
using System.Collections;

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

        if (smallCubeActive == null) {
            smallCubeActive = (GameObject)Instantiate(smallCubePrefab, smallCubeSpawn.position, Quaternion.identity);
        } else {
            smallCubeActive.transform.position = smallCubeSpawn.position;
        }
        if (bigCubeActive == null) {
            bigCubeActive = (GameObject)Instantiate(bigCubePrefab, bigCubeSpawn.position, Quaternion.identity);
        } else {
            bigCubeActive.transform.position = bigCubeSpawn.position;
        }
        Debug.LogError("Sanity check: " + (bigCubeActive == null) + ", " + (smallCubeActive == null));
    }

    public IEnumerator DestroyCubes() {
        yield return null;
        Destroy(smallCubeActive.gameObject);
        Debug.Log("Destroyed smallCube");
        yield return null;
        Destroy(bigCubeActive.gameObject);
        Debug.Log("Destroyed bigCube");
        yield return null;
        Debug.LogError("Sanity check: " + (bigCubeActive == null) + ", " + (smallCubeActive == null));
        smallCubeActive = null;
        bigCubeActive = null;
    }

    public System.Collections.Generic.List<Controller> GetControllers() {
        System.Collections.Generic.List<Controller> list = new System.Collections.Generic.List<Controller>() { smallCubeActive.GetComponent<Controller>(), bigCubeActive.GetComponent<Controller>() };
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

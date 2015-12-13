using UnityEngine;
using TwoTogether.Character;

public class SpawnHandler : MonoBehaviour {

	public GameObject smallCubePrefab;
	public GameObject bigCubePrefab;
	
	private Transform smallCubeSpawn;
	private Transform bigCubeSpawn;
	private GameObject smallCubeActive;
	private GameObject bigCubeActive;
    private static Controller[] controllers = new Controller[2];

    public delegate void RespawnEvent(CharacterType type, Transform spawnpoint);
    public static event RespawnEvent OnCharacterRespawn;

    public void Start() {
        smallCubeSpawn = transform.Find("SmallCubeSpawnpoint");
        bigCubeSpawn = transform.Find("BigCubeSpawnpoint");

        if (smallCubeActive == null) {
            smallCubeActive = (GameObject)Instantiate(smallCubePrefab, smallCubeSpawn.position, Quaternion.identity);
            controllers[0] = smallCubeActive.GetComponent<Controller>();
        } else {
            smallCubeActive.transform.position = smallCubeSpawn.position;
        }
        if (bigCubeActive == null) {
            bigCubeActive = (GameObject)Instantiate(bigCubePrefab, bigCubeSpawn.position, Quaternion.identity);
            controllers[1] = bigCubeActive.GetComponent<Controller>();
        } else {
            bigCubeActive.transform.position = bigCubeSpawn.position;
        }
    }

    public System.Collections.Generic.List<Controller> GetControllers() {
        System.Collections.Generic.List<Controller> list = new System.Collections.Generic.List<Controller>() { smallCubeActive.GetComponent<Controller>(), bigCubeActive.GetComponent<Controller>() };
        return list;
    }

    public static Controller[] GetAllControllers() {
        return controllers;
    }

    // Handles quitting and reset
	void Update() {
		if (Input.GetKeyDown(KeyCode.R)) {
			ResetBlueCube();
			ResetRedCube();
		}
	}

	public void ResetRedCube() {
        if (OnCharacterRespawn != null) {
            OnCharacterRespawn(CharacterType.SmallCube, smallCubeSpawn);
        }
		smallCubeActive.transform.position = smallCubeSpawn.position;
	}

	public void ResetBlueCube() {
        if (OnCharacterRespawn != null) {
            OnCharacterRespawn(CharacterType.BigCube, bigCubeSpawn);
        }
		bigCubeActive.transform.position = bigCubeSpawn.position;
	}
}

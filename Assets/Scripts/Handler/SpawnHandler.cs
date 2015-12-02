using UnityEngine;
using TwoTogether.Character;

public class SpawnHandler : MonoBehaviour {

	public GameObject smallCubePrefab;
	public GameObject bigCubePrefab;
	
	private Transform smallCubeSpawn;
	private Transform bigCubeSpawn;
	private GameObject smallCubeActive;
	private GameObject bigCubeActive;

    public delegate void RespawnEvent(CharacterType type, Transform spawnpoint);
    public static event RespawnEvent OnCharacterRespawn;

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
    }

    public System.Collections.Generic.List<Controller> GetControllers() {
        System.Collections.Generic.List<Controller> list = new System.Collections.Generic.List<Controller>() { smallCubeActive.GetComponent<Controller>(), bigCubeActive.GetComponent<Controller>() };
        return list;
    }

    // Handles quitting and reset
	void Update() {
		//if (Input.GetKeyDown(KeyCode.Escape)) {
		//	Application.Quit();
		//}
		if (Input.GetKeyDown(KeyCode.R)) {
			ResetBlueCube();
			ResetRedCube();
		}
	}

	public void ResetRedCube() {
        OnCharacterRespawn(CharacterType.SmallCube, smallCubeSpawn);
		smallCubeActive.transform.position = smallCubeSpawn.position;
	}

	public void ResetBlueCube() {
        OnCharacterRespawn(CharacterType.BigCube, bigCubeSpawn);
		bigCubeActive.transform.position = bigCubeSpawn.position;
	}
}

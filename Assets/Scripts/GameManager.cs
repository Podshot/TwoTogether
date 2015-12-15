using UnityEngine;
using TwoTogether.Character;
using System.Collections;

public class GameManager : MonoBehaviour {

    public GameObject prefab;

    [SerializeField] private bool smallCubeObjectiveMet = false;
    [SerializeField] private bool bigCubeObjectiveMet = false;
    private GameObject levelGameObject;
    private LevelIdentity levelSelection;

    // Use this for initialization
    void Awake() {
        GameObject go = GameObject.Find("LevelSelector");
        DownloadLevels levelDownloader = GameObject.Find("LevelDownloader").GetComponent<DownloadLevels>();
        if (go != null) {
            levelSelection = GameObject.Find("LevelSelector").GetComponent<LevelIdentity>();
            prefab = levelDownloader.Levels[levelSelection.GetIndex()];
        } else {
            GameObject selector = new GameObject("LevelSelector");
            selector.AddComponent<LevelIdentity>();
            levelSelection = selector.GetComponent<LevelIdentity>();
            levelSelection.SetID(0);
            DontDestroyOnLoad(selector);
            prefab = levelDownloader.Levels[levelSelection.GetIndex()];
        }
        levelGameObject = Instantiate(prefab);
        ConfigHandler.Config.SetField("Progress", levelSelection.GetIndex());
        ConfigHandler.SaveConfig();
        ObjectiveHandler.OnObjectiveActivated += ObjectiveActivated;
        ObjectiveHandler.OnObjectiveDeactivated += ObjectiveDeactivated;
	}
	
    public void ObjectiveActivated(CharacterType type) {
        if (type == CharacterType.SmallCube) {
            smallCubeObjectiveMet = true;
        } else {
            bigCubeObjectiveMet = true;
        }
    }

    public void ObjectiveDeactivated(CharacterType type) {
        if (type == CharacterType.SmallCube) {
            smallCubeObjectiveMet = false;
        } else {
            bigCubeObjectiveMet = false;
        }
    }

	// Update is called once per frame
	void Update () {
        if (smallCubeObjectiveMet && bigCubeObjectiveMet) {
            foreach (IFadeable fader in levelGameObject.GetComponentsInChildren(typeof(IFadeable))) {
                StartCoroutine(fader.FadeOut());
            }
            foreach (Controller controller in SpawnHandler.GetAllControllers()) {
                StartCoroutine(controller.FadeOut());
            }
            StartCoroutine(LoadNextLevel());
        }
	}

    IEnumerator LoadNextLevel() {
        yield return new WaitForSeconds(2f);
        levelSelection.SetID(levelSelection.GetIndex() + 1);
        //yield return null;
        Application.LoadLevel("PrefabScene");
        // Instantiate a new prefab
    }
}

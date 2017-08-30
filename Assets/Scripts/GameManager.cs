using UnityEngine;
using TwoTogether.Character;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public delegate void FadeEvent(TwoTogether.Fading.FadeType type, float value);
    public static event FadeEvent FadeActivated;

    [SerializeField] private bool smallCubeObjectiveMet = false;
    [SerializeField] private bool bigCubeObjectiveMet = false;

    // Use this for initialization
    void Awake() {
        ConfigHandler.LoadConfig();
        ConfigHandler.Config.SetField("Progress", int.Parse(SceneManager.GetActiveScene().name.Replace("Level_", "")));
        ConfigHandler.SaveConfig();
        ObjectiveHandler.OnObjectiveActivated += ObjectiveActivated;
        ObjectiveHandler.OnObjectiveDeactivated += ObjectiveDeactivated;
	}

    private void Start() {
        FadeActivated(TwoTogether.Fading.FadeType.FADE_IN, 0.0f);
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
            FadeActivated(TwoTogether.Fading.FadeType.FADE_OUT, 0.0f);
            /*
            //foreach (IFadeable fader in levelGameObject.GetComponentsInChildren(typeof(IFadeable))) {
            foreach (GameObject _fader in fadeables) {
                IFadeable fader = (IFadeable) _fader.GetComponent(typeof(IFadeable));
                StartCoroutine(fader.FadeOut());
            }
            foreach (Controller controller in SpawnHandler.GetAllControllers()) {
                StartCoroutine(controller.FadeOut());
            }
            */
            //StartCoroutine(LoadNextLevel());
        }
	}

    IEnumerator LoadNextLevel() {
        yield return new WaitForSeconds(2f);
        int nextLevel = (int)ConfigHandler.Config["Progress"].n;
        //yield return null;
        SceneManager.LoadScene("Level_" + nextLevel);
        // Instantiate a new prefab
    }
}

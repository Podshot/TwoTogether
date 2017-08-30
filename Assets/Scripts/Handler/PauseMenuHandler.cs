using TwoTogether.Character;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuHandler : MonoBehaviour {

    public GameObject parent;

    // State
    private bool inMenu = false;
    private int cooldown = 0;
    private GameObject terrain;
    private GameObject objectives;
    private GameObject active;
    private Text helpText;

    void Awake() {
        GameObject canvas = GameObject.Find("Canvas");
        active = (Instantiate(parent) as GameObject);
        active.SetActive(true);
        active.transform.position = new Vector3(0, -66, 0);
        active.transform.SetParent(canvas.transform, false);
        foreach (Button button in active.GetComponentsInChildren<Button>()) {
            string name = button.name;
            switch (name) {
                case "ResumeButton":
                    button.onClick.AddListener(() => { CloseMenu(); });
                    break;
                case "MainMenuButton":
                    button.onClick.AddListener(() => { LoadMainMenu(); });
                    break;
                case "QuitButton":
                    button.onClick.AddListener(() => { Quit(); });
                    break;
                default:
                    break;
            }
        }
        active.SetActive(false);
    }

    // Use this for initialization
    void Start() {
        terrain = GameObject.FindGameObjectWithTag("Terrain");
        objectives = GameObject.FindGameObjectWithTag("Objectives");
        GameObject go = GameObject.FindGameObjectWithTag("HelpText");
        if (go == null) {
            go = GameObject.Find("HelpText");
        }
        helpText = go.GetComponent<Text>();
    }

    void ShowMenu() {
        foreach (Controller controller in SpawnHandler.GetAllControllers()) {
            controller.PauseController(true);
        }
        terrain.GetComponent<TerrainFader>().PartialFade(0.5f);
        //objectives.GetComponent<ObjectiveHandler>().PartialFade(0.5f);
        helpText.color = new Color(helpText.color.r, helpText.color.g, helpText.color.b, 0.5f);
        active.SetActive(true);
        cooldown = 20;
        inMenu = true;
    }

    public void CloseMenu() {
        foreach (Controller controller in SpawnHandler.GetAllControllers()) {
            controller.PauseController(false);
        }
        terrain.GetComponent<TerrainFader>().PartialFade(1f);
        //objectives.GetComponent<ObjectiveHandler>().PartialFade(1f);
        helpText.color = new Color(helpText.color.r, helpText.color.g, helpText.color.b, 1f);
        active.SetActive(false);
        cooldown = 20;
        inMenu = false;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.P) && inMenu && cooldown == 0) {
            CloseMenu();
        }
        if (Input.GetKeyDown(KeyCode.P) && !inMenu && cooldown == 0) {
            ShowMenu();
        }

        if (inMenu) {
            if (Input.GetKeyDown(KeyCode.Q)) {
                Quit();
            }
            if (Input.GetKeyDown(KeyCode.M)) {
                LoadMainMenu();
            }
        }
        if (cooldown > 0) {
            cooldown--;
        }
    }

    public void LoadMainMenu() {
        Destroy(GameObject.Find("LevelDownloader"));
        Destroy(GameObject.Find("LevelSelector"));
        Application.LoadLevel("MainMenu");
    }

    public void Quit() {
        ConfigHandler.SaveConfig();
        Application.Quit();
    }
}

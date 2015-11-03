using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseMenuHandler : MonoBehaviour {

    public GameObject parent;

    // State
    private bool inMenu = false;
    private int cooldown = 0;
    private GameObject terrain;
    private Text helpText;

    // Use this for initialization
    void Start() {
        terrain = GameObject.FindGameObjectWithTag("Terrain");
    }

    void ShowMenu() {
        GameState state = Camera.main.GetComponent<GameState>();
        state.PauseControllers(true);
        state.PartiallyFadeCharactersAndObjectives(true);
        foreach (SpriteRenderer renderer in terrain.GetComponentsInChildren<SpriteRenderer>()) {
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0.5f);
        }
        helpText.color = new Color(helpText.color.r, helpText.color.g, helpText.color.b, 0.5f);
        parent.SetActive(true);
        cooldown = 20;
        inMenu = true;
    }

    public void CloseMenu() {
        GameState state = Camera.main.GetComponent<GameState>();
        state.PauseControllers(false);
        state.PartiallyFadeCharactersAndObjectives(false);
        foreach (SpriteRenderer renderer in terrain.GetComponentsInChildren<SpriteRenderer>()) {
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1f);
        }
        helpText.color = new Color(helpText.color.r, helpText.color.g, helpText.color.b, 1f);
        parent.SetActive(false);
        cooldown = 20;
        inMenu = false;
    }

    // Update is called once per frame
    void Update() {
        if (helpText == null) {
            helpText = Camera.main.GetComponent<GameState>().GetHelpText();
        }
        if (Input.GetKeyDown(KeyCode.P) && inMenu && cooldown == 0) {
            CloseMenu();
        }
        if (Input.GetKeyDown(KeyCode.P) && !inMenu && cooldown == 0) {
            ShowMenu();
        }

        if (inMenu) {
            if (Input.GetKeyDown(KeyCode.Q)) {
                Application.Quit();
            }
            if (Input.GetKeyDown(KeyCode.M)) {
                LoadMainMenu();
            }
        }
        if (cooldown > 0) {
            cooldown--;
        }
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.C)) {
            Application.CaptureScreenshot(Camera.main.GetComponent<LoadLevel>().GetLevelName() + ".png");
        }
#endif
    }

    public void LoadMainMenu() {
        Application.LoadLevel("MainMenu");
    }

    public void Quit() {
        Application.Quit();
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuHandler : MonoBehaviour {

	public GameObject startGameButton;
	public GameObject controlsButton;
	public GameObject backButton;
    public GameObject selectLevelsButton;
	public Text titleText;
	public Text authorText;
	public Text controlText;
	public Text versionText;
	public Text howToPlayText;
	public GameObject staticSprites;

	private string state = "mainmenu";

    // Called when the "Start Game" button is pressed
    public void OnClickStartGameButton() {
		startGameButton.GetComponent<Button>().enabled = false;
		startGameButton.GetComponent<Image>().CrossFadeAlpha(0f, 0.5f, false);
		startGameButton.GetComponentInChildren<Text>().CrossFadeAlpha(0f, 0.5f, false);

        selectLevelsButton.GetComponent<Button>().enabled = false;
        selectLevelsButton.GetComponent<Image>().CrossFadeAlpha(0f, 0.5f, false);
        selectLevelsButton.GetComponentInChildren<Text>().CrossFadeAlpha(0f, 0.5f, false);

        controlsButton.GetComponent<Button>().enabled = false;
		controlsButton.GetComponent<Image>().CrossFadeAlpha(0f, 0.5f, false);
		controlsButton.GetComponentInChildren<Text>().CrossFadeAlpha(0f, 0.5f, false);

		titleText.CrossFadeAlpha(0f, 0.5f, false);
		authorText.CrossFadeAlpha(0f, 0.5f, false);
		controlText.CrossFadeAlpha(0f, 0.5f, false);
		versionText.CrossFadeAlpha(0f, 0.5f, false);
		StartCoroutine(WaitAndLoad("TestScene"));
	}

    // Handles enabling and disabling UI components when the "Controls & Tutorial" button is pressed
    public void OnClickControlsButton() {
		state = "controls";
		startGameButton.GetComponent<Button>().enabled = false;
		startGameButton.GetComponent<Image>().enabled = false;
		startGameButton.GetComponentInChildren<Text>().enabled = false;

        selectLevelsButton.GetComponent<Button>().enabled = false;
        selectLevelsButton.GetComponent<Image>().enabled = false;
        selectLevelsButton.GetComponentInChildren<Text>().enabled = false;

        titleText.enabled = false;
		authorText.enabled = false;
		versionText.enabled = false;
		controlsButton.GetComponent<Button>().enabled = false;
		controlsButton.GetComponent<Image>().enabled = false;
		controlsButton.GetComponentInChildren<Text>().enabled = false;

		controlText.enabled = true;
		backButton.GetComponent<Button>().enabled = true;
		backButton.GetComponent<Image>().enabled = true;
		backButton.GetComponentInChildren<Text>().enabled = true;
		howToPlayText.enabled = true;
		foreach (SpriteRenderer renderer in staticSprites.GetComponentsInChildren<SpriteRenderer>()) {
			renderer.enabled = true;
		}
	}

    // Handles enabling and disabling UI components when the "Back" button is pressed
	public void OnClickBackButton() {
		if (state == "controls") {
			state = "mainmenu";
			controlText.enabled = false;
			backButton.GetComponent<Button>().enabled = false;
			backButton.GetComponent<Image>().enabled = false;
			backButton.GetComponentInChildren<Text>().enabled = false;
			howToPlayText.enabled = false;
			foreach (SpriteRenderer renderer in staticSprites.GetComponentsInChildren<SpriteRenderer>()) {
				renderer.enabled = false;
			}

			startGameButton.GetComponent<Button>().enabled = true;
			startGameButton.GetComponent<Image>().enabled = true;
			startGameButton.GetComponentInChildren<Text>().enabled = true;

			titleText.enabled = true;
			authorText.enabled = true;
			versionText.enabled = true;

			controlsButton.GetComponent<Button>().enabled = true;
			controlsButton.GetComponent<Image>().enabled = true;
			controlsButton.GetComponentInChildren<Text>().enabled = true;

            selectLevelsButton.GetComponent<Button>().enabled = true;
            selectLevelsButton.GetComponent<Image>().enabled = true;
            selectLevelsButton.GetComponentInChildren<Text>().enabled = true;
        }
	}

    public void OnClickSelectLevel() {
        startGameButton.GetComponent<Button>().enabled = false;
        startGameButton.GetComponent<Image>().CrossFadeAlpha(0f, 0.5f, false);
        startGameButton.GetComponentInChildren<Text>().CrossFadeAlpha(0f, 0.5f, false);

        selectLevelsButton.GetComponent<Button>().enabled = false;
        selectLevelsButton.GetComponent<Image>().CrossFadeAlpha(0f, 0.5f, false);
        selectLevelsButton.GetComponentInChildren<Text>().CrossFadeAlpha(0f, 0.5f, false);

        controlsButton.GetComponent<Button>().enabled = false;
        controlsButton.GetComponent<Image>().CrossFadeAlpha(0f, 0.5f, false);
        controlsButton.GetComponentInChildren<Text>().CrossFadeAlpha(0f, 0.5f, false);

        titleText.CrossFadeAlpha(0f, 0.5f, false);
        authorText.CrossFadeAlpha(0f, 0.5f, false);
        controlText.CrossFadeAlpha(0f, 0.5f, false);
        versionText.CrossFadeAlpha(0f, 0.5f, false);

        StartCoroutine(WaitAndLoad("LevelSelect"));
    }

    // Opens a webbrowser to the alpha downloads page
	public void OnClickUpdateGame() {
		Application.OpenURL("http://podshot.github.io/TwoTogether/download");
	}


    // Waits 0.75 seconds before loading the first level
	IEnumerator WaitAndLoad(string scene) {
        yield return new WaitForSeconds(0.75f);
        Application.LoadLevel(scene);
	}
}

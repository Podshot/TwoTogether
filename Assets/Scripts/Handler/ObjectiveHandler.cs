using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Stopwatch = System.Diagnostics.Stopwatch;

public class ObjectiveHandler : MonoBehaviour {

	private Text helpText;
	public GameObject[] fadeables;
	public GameObject redObjective;
	public GameObject blueObjective;
	public string nextLevel;

	private bool smallCubeActivated;
	private bool bigCubeActivated;
	private bool started = false;
	private bool fadeIn = false;
	private bool fadeOut = false;
	private SpriteRenderer redRenderer;
	private SpriteRenderer blueRenderer;
	private bool readyToSwitch = false;
    private bool toSwitch = false;
    private GameState gameState;    

    // Fades in objectives and the help text
	public void Load() {
        gameState = Camera.main.GetComponent<GameState>();

		redRenderer = redObjective.GetComponent<SpriteRenderer>();
		blueRenderer = blueObjective.GetComponent<SpriteRenderer>();

        //helpText.color = new Color(helpText.color.r, helpText.color.g, helpText.color.b, 0.002f);
        helpText = gameState.GetHelpText();
        helpText.color = new Color(helpText.color.r, helpText.color.g, helpText.color.b, 0.002f);
        redRenderer.color = new Color(redRenderer.color.r, redRenderer.color.g, redRenderer.color.b, 0.002f);
		blueRenderer.color = new Color(blueRenderer.color.r, blueRenderer.color.g, blueRenderer.color.b, 0.002f);
        redRenderer.enabled = true;
        blueRenderer.enabled = true;
		//yield return new WaitForSeconds(2);
		//FadeIn();
		fadeables[1] = GameObject.FindGameObjectWithTag("BigCube");
		fadeables[2] = GameObject.FindGameObjectWithTag("SmallCube");
	}

    // Public callback to register completion of an objective
	public void SetObjectiveActivated(string cubeType, bool activated) {
		if (cubeType == "SmallCube") {
			smallCubeActivated = activated;
		} else {
			bigCubeActivated = activated;
		}
	}

	void Update() {
        /*
		if (fadeIn) {
			if (!(blueRenderer.color.a > 0.70196078431372549019607843137255f)) {
				Color oldColor = blueRenderer.color;
				oldColor.a *= 1.0625f;
				blueRenderer.color = oldColor;
			}
			if (!(redRenderer.color.a > 0.61960784313725490196078431372549f)) {
				Color oldColor = redRenderer.color;
				oldColor.a *= 1.0625f;
				redRenderer.color = oldColor;
			}
            // CrossFadeAlpha doesn't work when fading in 
			Color textColor = helpText.color;
			textColor.a *= 1.0625f;
			helpText.color = textColor;
			if (redRenderer.color.a > 0.61960784313725490196078431372549f && blueRenderer.color.a > 0.70196078431372549019607843137255f) {
				fadeIn = false;
			}
		}

		if (fadeOut) {
			if (!(blueRenderer.color.a < 0.002f)) {
				Color oldColor = blueRenderer.color;
				oldColor.a /= 1.0625f;
				blueRenderer.color = oldColor;
			}
			if (!(redRenderer.color.a < 0.002f)) {
				Color oldColor = redRenderer.color;
				oldColor.a /= 1.0625f;
				redRenderer.color = oldColor;
			}
			if (redRenderer.color.a < 0.002f && blueRenderer.color.a < 0.002f) {
				fadeOut = false;
				readyToSwitch = true;
			}
		}
        */

		if (smallCubeActivated && bigCubeActivated) {
            Debug.Log("Both objectives met");
            Debug.Log("Stopping controllers");
            gameState.StopControllers();
            Debug.Log("Stopped controllers");
            if (!started) {
				started = true;
                //StartCoroutine(gameState.Switch());
                Debug.Log("Switching");
                gameState._Switch();
                Debug.Log("Switched");
                //StartCoroutine(WaitAndLoad());
            }
		}
		//if (readyToSwitch) {
			//if (Input.GetKeyDown(KeyCode.E)) {
		//	StartCoroutine(WaitAndLoad());
			//}
		//}
        
	}

    public IEnumerator FadeOut() {
        for (float i = 1; i > 0f; i -= 0.0005f) {
            Color b_color = blueRenderer.color;
            b_color.a -= i;
            blueRenderer.color = b_color;

            Color r_color = redRenderer.color;
            r_color.a -= i;
            redRenderer.color = r_color;

            yield return null;
        }
    }

    public IEnumerator FadeIn() {
        for (float i = 0; i < 1f; i += 0.0005f) {
            Color b_color = blueRenderer.color;
            b_color.a += i;
            blueRenderer.color = b_color;

            Color r_color = redRenderer.color;
            r_color.a += i;
            redRenderer.color = r_color;

            yield return null;
        }
    }

    //public void FadeIn() {
    //	fadeIn = true;
    //}

    // Handles fading out of all level components
    //public void FadeOut() {
    //yield return new WaitForSeconds(0.125f);
    //helpText.CrossFadeAlpha(0f, 0.5f, false);
    //fadeOut = true;
		//foreach (GameObject obj in fadeables) {
		//	if (obj.tag == "Terrain") {
		//		obj.GetComponent<TerrainFader>().FadeOut();
		//	}
		//	if (obj.tag.Contains("Cube")) {
		//		obj.GetComponent<Controller>().FadeOut();
		//	}
		//}
	//}

    // Loads next level after a short delay
    IEnumerator WaitAndLoad() {
        yield return new WaitForSeconds(3f);
        toSwitch = true;
        /*
        if (Application.CanStreamedLevelBeLoaded(nextLevel))
        {
            yield return new WaitForSeconds(0.5f);
            Application.LoadLevel(nextLevel);
        } else
        {
            yield return new WaitForSeconds(0.0f);
        }
        */
	}
}

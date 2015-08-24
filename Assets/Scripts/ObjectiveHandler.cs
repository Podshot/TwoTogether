using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Stopwatch = System.Diagnostics.Stopwatch;

public class ObjectiveHandler : MonoBehaviour {

	public Text helpText;
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

	IEnumerator Start() {
		redRenderer = redObjective.GetComponent<SpriteRenderer>();
		blueRenderer = blueObjective.GetComponent<SpriteRenderer>();

		helpText.color = new Color(helpText.color.r, helpText.color.g, helpText.color.b, 0.002f);
		redRenderer.color = new Color(redRenderer.color.r, redRenderer.color.g, redRenderer.color.b, 0.002f);
		blueRenderer.color = new Color(blueRenderer.color.r, blueRenderer.color.g, blueRenderer.color.b, 0.002f);
		yield return new WaitForSeconds(2);
		FadeIn();
		fadeables[1] = GameObject.FindGameObjectWithTag("BigCube");
		fadeables[2] = GameObject.FindGameObjectWithTag("SmallCube");
	}

	public void SetObjectiveActivated(string cubeType, bool activated) {
		if (cubeType == "SmallCube") {
			smallCubeActivated = activated;
		} else {
			bigCubeActivated = activated;
		}
	}

	void Update() {
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

		if (smallCubeActivated && bigCubeActivated) {
			if (!started) {
				started = true;
				StartCoroutine(FadeOut());
			}
		}
		if (readyToSwitch) {
			//if (Input.GetKeyDown(KeyCode.E)) {
			StartCoroutine(WaitAndLoad());
			//}
		}
	}

	public void FadeIn() {
		fadeIn = true;
	}

	IEnumerator /*void*/ FadeOut() {
		yield return new WaitForSeconds(0.125f);
		helpText.CrossFadeAlpha(0f, 0.5f, false);
		fadeOut = true;
		foreach (GameObject obj in fadeables) {
			if (obj.tag == "Terrain") {
				obj.GetComponent<TerrainFader>().FadeOut();
			}
			if (obj.tag == "BigCube") {
				obj.GetComponent<BigCubeController>().FadeOut();
			}
			if (obj.tag == "SmallCube") {
				obj.GetComponent<SmallCubeController>().FadeOut();
			}
		}
	}

    IEnumerator WaitAndLoad() {
        if (Application.CanStreamedLevelBeLoaded(nextLevel))
        {
            yield return new WaitForSeconds(0.5f);
            Application.LoadLevel(nextLevel);
        } else
        {
            yield return new WaitForSeconds(0.0f);
        }
	}
}

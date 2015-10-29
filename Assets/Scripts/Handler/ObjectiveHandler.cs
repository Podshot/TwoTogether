using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Stopwatch = System.Diagnostics.Stopwatch;

public class ObjectiveHandler : MonoBehaviour, IFadeable {

    private Text helpText;
    public GameObject[] fadeables;
    public GameObject redObjective;
    public GameObject blueObjective;
    public string nextLevel;

    [SerializeField]
    private bool smallCubeActivated;
    [SerializeField]
    private bool bigCubeActivated;
    [SerializeField]
    private bool started = false;
    private SpriteRenderer redRenderer;
    private SpriteRenderer blueRenderer;
    private GameState gameState;

    // Fades in objectives and the help text
    public void Load() {
        gameState = Camera.main.GetComponent<GameState>();

        redRenderer = redObjective.GetComponent<SpriteRenderer>();
        blueRenderer = blueObjective.GetComponent<SpriteRenderer>();

        //helpText.color = new Color(helpText.color.r, helpText.color.g, helpText.color.b, 0.002f);
        helpText = gameState.GetHelpText();
        helpText.color = new Color(helpText.color.r, helpText.color.g, helpText.color.b, 0.0f);
        redRenderer.color = new Color(redRenderer.color.r, redRenderer.color.g, redRenderer.color.b, 0.002f);
        blueRenderer.color = new Color(blueRenderer.color.r, blueRenderer.color.g, blueRenderer.color.b, 0.002f);
        redRenderer.enabled = true;
        blueRenderer.enabled = true;
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
        if (smallCubeActivated && bigCubeActivated) {
            if (!started) {
                started = true;
                gameState.StopControllers();
                StartCoroutine(gameState.Switch());
            }
        }
    }

    public void Reset() {
        smallCubeActivated = false;
        bigCubeActivated = false;
        started = false;
    }

    public IEnumerator FadeOut() {
        for (float i = 1; i > 0f; i -= 0.025f) {
            Color b_color = blueRenderer.color;
            b_color.a = i;
            blueRenderer.color = b_color;

            Color r_color = redRenderer.color;
            r_color.a = i;
            redRenderer.color = r_color;

            yield return null;
        }
    }

    public IEnumerator FadeIn() {
        for (float i = 0; i < 1f; i += 0.05f) {
            Color b_color = blueRenderer.color;
            b_color.a = i;
            blueRenderer.color = b_color;

            Color r_color = redRenderer.color;
            r_color.a = i;
            redRenderer.color = r_color;

            yield return null;
        }
    }
}

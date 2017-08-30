using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TwoTogether.Character;
using System;
using TwoTogether.Fading;

public class ObjectiveHandler : IFader {

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

    public delegate void ObjectiveEvent(CharacterType characterType);
    public static event ObjectiveEvent OnObjectiveActivated;
    public static event ObjectiveEvent OnObjectiveDeactivated;

    void Start() {
        redRenderer = redObjective.GetComponent<SpriteRenderer>();
        blueRenderer = blueObjective.GetComponent<SpriteRenderer>();

        //helpText.color = new Color(helpText.color.r, helpText.color.g, helpText.color.b, 0.002f);
        //helpText = gameState.GetHelpText();
        //helpText.color = new Color(helpText.color.r, helpText.color.g, helpText.color.b, 0.0f);
        redRenderer.color = new Color(redRenderer.color.r, redRenderer.color.g, redRenderer.color.b, 0.002f);
        blueRenderer.color = new Color(blueRenderer.color.r, blueRenderer.color.g, blueRenderer.color.b, 0.002f);
        redRenderer.enabled = true;
        blueRenderer.enabled = true;
        fadeables[1] = GameObject.FindGameObjectWithTag("BigCube");
        fadeables[2] = GameObject.FindGameObjectWithTag("SmallCube");

        //StartCoroutine(FadeIn());
    }

    // Public callback to register completion of an objective
    public void SetObjectiveActivated(CharacterType type, bool activated) {
        if (activated && OnObjectiveActivated != null) {
            OnObjectiveActivated(type);
        }
        if (!activated && OnObjectiveDeactivated != null) {
            OnObjectiveDeactivated(type);
        }
        if (type == CharacterType.SmallCube) {
            smallCubeActivated = activated;
        } else {
            bigCubeActivated = activated;
        }
    }

    void Update() {
        //if (smallCubeActivated && bigCubeActivated) {
        //    if (!started) {
        //        started = true;
                //gameState.StopControllers();
                //StartCoroutine(gameState.Switch());
       //     }
       // }
    }

    public void Reset() {
        smallCubeActivated = false;
        bigCubeActivated = false;
        started = false;
        OnObjectiveActivated = null;
        OnObjectiveDeactivated = null;
    }

    public override IEnumerator FadeOut() {
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

    
    public override IEnumerator FadeIn() {
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

    public override void PartialFade(float alpha) {
        blueRenderer.color = new Color(blueRenderer.color.r, blueRenderer.color.g, blueRenderer.color.b, alpha);
        redRenderer.color = new Color(redRenderer.color.r, redRenderer.color.g, redRenderer.color.b, alpha);
    }
}

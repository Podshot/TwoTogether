using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TwoTogether.Fading;

public class TextFader : Fadeable {

    private Text text;

    public void Awake() {
        base.Awake();
        text = GetComponent<Text>();
    }

    public void Start() {
        
    }

    // Public fuction for fading in the character
    public override IEnumerator FadeIn() {
        for (float i = 0; i < 1f; i += 0.025f) {
            Color color = text.color;
            color.a = i;
            text.color = color;
            yield return null;
        }
        yield break;
    }

    // Public fuction for fading out the character
    public override IEnumerator FadeOut() {
        for (float i = 1; i > 0f; i -= 0.025f) {
            Color color = text.color;
            color.a = i;
            text.color = color;
            yield return null;
        }
        yield break;
    }

    public override void PartialFade(float alpha) {
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
    }
}

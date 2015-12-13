using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextFader : MonoBehaviour, IFadeable {

    private Text text;

    public void Start() {
        text = GetComponent<Text>();
    }

    // Public fuction for fading in the character
    public IEnumerator FadeIn() {
        for (float i = 0; i < 1f; i += 0.025f) {
            Color color = text.color;
            color.a = i;
            text.color = color;
            yield return null;
        }
        yield break;
    }

    // Public fuction for fading out the character
    public IEnumerator FadeOut() {
        for (float i = 1; i > 0f; i -= 0.025f) {
            Color color = text.color;
            color.a = i;
            text.color = color;
            yield return null;
        }
        yield break;
    }

    public void PartiallyFade(float alpha) {
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
    }
}

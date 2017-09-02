using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Stopwatch = System.Diagnostics.Stopwatch;
using System;
using TwoTogether.Fading;

public class TerrainFader : Fadeable {

	private SpriteRenderer[] spriteRenderers;

    void Awake() {
        base.Awake();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer renderer in spriteRenderers) {
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0.0f);
        }
        //StartCoroutine(FadeIn());
    }

    public void Cleanup() {
        spriteRenderers = null;
    }
	
    // Fades the terrain in and out. Fading happens manually, since CrossFadeAlpha only works for Text component
    public override IEnumerator FadeOut() {
        for (float i = 1; i > 0f; i -= 0.025f) {
            foreach (SpriteRenderer renderer in spriteRenderers) {
                if (renderer != null) {
                    Color color = renderer.color;
                    color.a = i;
                    renderer.color = color;
                }
            }
            yield return null;
        }
        yield break;
    }

    public override IEnumerator FadeIn() {
        for (float i = 0; i < 1f; i += 0.025f) {
            foreach (SpriteRenderer renderer in spriteRenderers) {
                if (renderer != null) {
                    Color color = renderer.color;
                    color.a = i;
                    renderer.color = color;
                }
            }
            yield return null;
        }
        yield break;
    }

    public override void PartialFade(float alpha) {
        foreach (SpriteRenderer renderer in spriteRenderers) {
            if (renderer != null) {
                renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, alpha);
            }
        }
    }
}

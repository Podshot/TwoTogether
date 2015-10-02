using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TerrainFader : MonoBehaviour {

	private SpriteRenderer[] spriteRenderers;
	private bool fadeIn = false;
	private bool fadeOut = false;
    private bool ready = false;

    // Sets all child SpriteRenders to have a low alpha, then after 2 seconds, fades all of them in
    /*
	IEnumerator Start () {
		spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer renderer in spriteRenderers) {
			renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0.002f);
		}
		yield return new WaitForSeconds(2);
		FadeIn();
	
	}
    */
    public void Load() {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer renderer in spriteRenderers) {
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0.0f);
        }
    }

    public bool GetFadeOut() {
        return ready;
    }

    public void Cleanup() {
        spriteRenderers = null;
    }
	
    // Fades the terrain in and out. Fading happens manually, since CrossFadeAlpha only works for Text components
    /*
	void Update () {
		if (fadeIn) {
			foreach (SpriteRenderer renderer in spriteRenderers) {
				if (renderer.color.a != 0) {
					Color color = renderer.color;
					color.a *= 1.0625f;
					renderer.color = color;
				}
			}
			if (spriteRenderers[spriteRenderers.Length - 1].color.a > 1.0f) {
				fadeIn = false;
			}
		}
		if (fadeOut) {
			foreach(SpriteRenderer renderer in spriteRenderers) {
				if (renderer.color.a != 0) {
					Color color = renderer.color;
					color.a /= 1.0625f;
					renderer.color = color;
				}
			}
			if (spriteRenderers[spriteRenderers.Length - 1].color.a < 0.002f) {
				fadeOut = false;
			}
		}
	}
    */

    public IEnumerator FadeOut() {
        for (float i = 1; i > 0f; i -= 0.0005f) {
            foreach (SpriteRenderer renderer in spriteRenderers) {
                Color color = renderer.color;
                color.a -= i;
                renderer.color = color;
            }
            yield return null;
        }
        ready = true;
    }

    public IEnumerator FadeIn() {
        for (float i = 0; i < 1f; i += 0.0005f) {
            foreach (SpriteRenderer renderer in spriteRenderers) {
                Color color = renderer.color;
                color.a += i;
                renderer.color = color;
            }
            yield return null;
        }
    }
}

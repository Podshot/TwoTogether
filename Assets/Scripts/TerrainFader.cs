using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TerrainFader : MonoBehaviour {

	private SpriteRenderer[] spriteRenderers;
	private bool fadeIn = false;
	private bool fadeOut = false;

    // Sets all child SpriteRenders to have a low alpha, then after 2 seconds, fades all of them in
	IEnumerator Start () {
		spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer renderer in spriteRenderers) {
			renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0.002f);
		}
		yield return new WaitForSeconds(2);
		FadeIn();
	
	}
	
    // Fades the terrain in and out. Fading happens manually, since CrossFadeAlpha only works for Text components
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

    // Public method to activate fading in
	public void FadeIn() {
		fadeIn = true;
	}

    // Public method to activate fading out
	public void FadeOut() {
		fadeOut = true;
	}
}

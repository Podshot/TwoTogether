using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TerrainFader : MonoBehaviour {

	private SpriteRenderer[] spriteRenderers;
	private bool fadeIn = false;
	private bool fadeOut = false;

	// Use this for initialization
	IEnumerator Start () {
		spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer renderer in spriteRenderers) {
			renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0.002f);
		}
		yield return new WaitForSeconds(2);
		FadeIn();
	
	}
	
	// Update is called once per frame
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

	public void FadeIn() {
		fadeIn = true;
	}

	public void FadeOut() {
		fadeOut = true;
	}
}

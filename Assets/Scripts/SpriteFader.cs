using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFader : MonoBehaviour {

	[SerializeField] private float inAlpha = 1.0f;
	[SerializeField] private float outAlpha = 0.002f;

	private static readonly float stepSize = 1.0625f;

	private bool fadeIn;
	private bool fadeOut;

	private SpriteRenderer spriteRenderer;

	void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Update() {
		/**
		 * This is a little counterintuitive, so I'll explain it.
		 * While bool fadeIn/fadeOut is true, the corrisponding method FadeIn/FadeOut is ran.
		 * FadeIn/FadeOut returns TRUE when complete.
		 * Thus, fadeIn/fadeOut are set to the negation of FadeIn/FadeOut's return value.
		 */
		if (fadeIn) {
			fadeIn = !FadeIn(spriteRenderer, inAlpha);
		}
		if (fadeOut) {
			fadeOut = !FadeOut(spriteRenderer, outAlpha);
		}
	}

	public bool IsFading() {
		return fadeIn || fadeOut;
	}

	public void FadeIn() {
		fadeIn = true;
	}

	public void FadeOut() {
		fadeOut = true;
	}

	/**
	 * Fades in a SpriteRenderer one step towards targetAlpha.
	 * @return TRUE if complete
	 * @return FALSE if in-process
	 */
	public static bool FadeIn(SpriteRenderer sprite, float targetAlpha) {
		Color c = sprite.color;
		c.a *= stepSize;
		sprite.color = c;
		return sprite.color.a > targetAlpha;
	}

	/**
	 * Fades out a SpriteRenderer one step towards targetAlpha.
	 * @return TRUE if complete
	 * @return FALSE if in-process
	 */
	public static bool FadeOut(SpriteRenderer sprite, float targetAlpha) {
		Color c = sprite.color;
		c.a /= stepSize;
		sprite.color = c;
		return sprite.color.a < targetAlpha;
	}
}

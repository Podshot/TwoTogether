using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class ScreenFader : MonoBehaviour {

	[SerializeField] Sprite flatSprite;

	private Camera cam;
	private GameObject fader;
	private SpriteRenderer spriteRenderer;

	void Awake() {
		cam = GetComponent<Camera>();
		fader = new GameObject("Screen Fader", typeof(SpriteFader));
		spriteRenderer = fader.GetComponent<SpriteRenderer>();

		fader.transform.parent = this.transform;
		fader.transform.localPosition = new Vector3 (0, 0, 10);
		fader.transform.localScale = new Vector3 (20, 20, 1);

		spriteRenderer.sprite = flatSprite;
		spriteRenderer.sortingOrder = 10;

		Color c = cam.backgroundColor;
		c.a = 1;
		spriteRenderer.color = c;
	}

	IEnumerator Start() {
		yield return new WaitForSeconds(2);
		fader.GetComponent<SpriteFader>().FadeOut();
	}

	public void FadeIn() {
		fader.GetComponent<SpriteFader> ().FadeOut ();
	}
	public void FadeOut() {
		fader.GetComponent<SpriteFader> ().FadeIn ();
	}
}

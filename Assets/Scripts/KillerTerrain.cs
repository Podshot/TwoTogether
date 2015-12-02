using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class KillerTerrain : MonoBehaviour, IFadeable, ILoadable {

	[SerializeField] private string targetTag = "none";

	private SpawnHandler handler;
    private SpriteRenderer spriteRenderer;

    public void Load() {
		handler = GameObject.FindGameObjectWithTag("SpawnController").GetComponent<SpawnHandler>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.0f);
	}

    public void Unload() { }

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag.Equals(targetTag)) {
			if (targetTag.Equals("BigCube")) {
				handler.ResetBlueCube();
			} else if (targetTag.Equals("SmallCube")) {
				handler.ResetRedCube();
			} else {
				print(string.Format("This shouldn't ever display. TargetTag={0}", targetTag));
			}
		}
	}

    public string GetTargetTag() {
        return targetTag;
    }

    public IEnumerator FadeOut() {
        for (float i = 1; i > 0f; i -= 0.025f) {
            if (spriteRenderer != null) {
                Color color = spriteRenderer.color;
                color.a = i;
                spriteRenderer.color = color;
            }
            yield return null;
        }
        yield break;
    }

    public IEnumerator FadeIn() {
        for (float i = 0; i < 1f; i += 0.025f) {
            if (spriteRenderer != null) {
                Color color = spriteRenderer.color;
                color.a = i;
                spriteRenderer.color = color;
            }
            yield return null;
        }
        yield break;
    }
}

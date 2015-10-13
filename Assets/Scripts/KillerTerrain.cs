using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class KillerTerrain : MonoBehaviour {

	[SerializeField] private string targetTag = "none";

	private SpawnHandler handler;
    private SpriteRenderer renderer;

	public void Load() {
		handler = GameObject.FindGameObjectWithTag("SpawnController").GetComponent<SpawnHandler>();
        renderer = GetComponent<SpriteRenderer>();
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0.0f);
	}

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
            if (renderer != null) {
                Color color = renderer.color;
                color.a = i;
                renderer.color = color;
            }
            yield return null;
        }
        yield break;
    }

    public IEnumerator FadeIn() {
        for (float i = 0; i < 1f; i += 0.025f) {
            if (renderer != null) {
                Color color = renderer.color;
                color.a = i;
                renderer.color = color;
            }
            yield return null;
        }
        yield break;
    }
}

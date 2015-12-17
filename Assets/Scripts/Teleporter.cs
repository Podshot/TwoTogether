using UnityEngine;
using System.Collections;
using TwoTogether.Character;

public class Teleporter : MonoBehaviour, IFadeable {

    public Transform targetDestination;
    public CharacterType targetCharacter;
    public float height = 5.5f;

    private Transform captured;
    private bool[] stages = new bool[4];
    private SpriteRenderer spriteRenderer;
    private Controller capturedController;

    // Use this for initialization
    void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(FadeIn());
	}
	
	// Update is called once per frame
	void Update () {
        if (stages[0]) {
            captured.Translate(Vector3.up * Time.deltaTime * 12.5f);
            if (Vector3.Distance(new Vector3(0f, captured.transform.position.y), new Vector3(0f, transform.position.y + height)) > height) {
                stages[0] = false;
                stages[1] = true;
            }
        }
        if (stages[1]) {
            captured.position = new Vector3(targetDestination.position.x, captured.position.y, captured.position.z);
            stages[1] = false;
            stages[2] = true;
        }
        if (stages[2]) {
            captured.Translate(Vector3.down * Time.deltaTime * 12.5f);
            if (Vector3.Distance(new Vector3(0f, captured.position.y), new Vector3(0f, targetDestination.position.y)) < 0.155f) {
                stages[2] = false;
                stages[3] = true;
            }
        }
        if (stages[3]) {
            capturedController.PauseController(false);
            capturedController = null;
            captured = null;
            stages[3] = false;
        }
	}

    IEnumerator WaitAndExecute(Collider2D collider) {
        yield return new WaitForSeconds(0.1f);
        Controller controller = collider.gameObject.GetComponent<Controller>();
        if (controller.Type == targetCharacter) {
            captured = collider.gameObject.transform;
            capturedController = controller;
            capturedController.PauseController(true);
            stages[0] = true;
        }
        yield break;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        StartCoroutine(WaitAndExecute(collider));
        /*
        Controller controller = collider.gameObject.GetComponent<Controller>();
        if (controller.Type == targetCharacter) {
            captured = collider.gameObject.transform;
            capturedController = controller;
            capturedController.PauseController(true);
            stages[0] = true;
        }
        */
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

    public void PartiallyFade(float alpha) {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
    }
}

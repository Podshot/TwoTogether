using UnityEngine;
using System.Collections;
using TwoTogether.Character;

[SerializePrivateVariables]
public class Objective : MonoBehaviour {

	public GameObject handler;
	public GameObject lookingFor;
    public bool running = true;

	private ObjectiveHandler objectiveHandler;
	private Color originalColor;
	private string lookingForTag;
	private SpriteRenderer spriteRenderer;
    private CharacterType lookingForType;

    private float startTime = 0.0f;
    private Vector3 originalPosition;

    // Gets the ObjectiveHandler component and sets the original color of the objective
    void Start () {
		objectiveHandler = handler.GetComponent<ObjectiveHandler>();
		spriteRenderer = GetComponent<SpriteRenderer>();
        if (lookingFor.tag == CharacterType.SmallCube.GetID()) {
            originalColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.61960784313725490196078431372549f);
        } else if (lookingFor.tag == CharacterType.BigCube.GetID()) {
            originalColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.70196078431372549019607843137255f);
        }
		lookingForTag = lookingFor.tag;
        lookingForType = CharacterTypeExtension.GetEnumFromID(lookingForTag);
    }

	void OnTriggerEnter2D(Collider2D otherObject) {
		if (otherObject.transform.tag == lookingForTag) {
			spriteRenderer.color = Color.black;
            if (startTime == 0.0f) {
                startTime = Time.time;
                originalPosition = otherObject.transform.position;
                StartCoroutine(SnapIn(otherObject.gameObject));
            }
            //otherObject.transform.position = transform.position;
			objectiveHandler.SetObjectiveActivated(lookingForType, true);
		}
	}

	void OnTriggerExit2D(Collider2D otherObject) {
		if (otherObject.tag == lookingForTag) {
			spriteRenderer.color = originalColor;
			objectiveHandler.SetObjectiveActivated(lookingForType, false);
            if (startTime != 0.0f) {
                startTime = 0.0f;
                originalPosition = new Vector3(0f, 0f, 0f);
            }
        }
	}

    private bool CheckAxis() {
        if (lookingForType == CharacterType.BigCube) {
            return ((Input.GetAxisRaw("HorizontalBig") == 0f) && (Input.GetAxisRaw("VerticalBig") == 0f));
        } else {
            return ((Input.GetAxisRaw("HorizontalSmall") == 0f) && (Input.GetAxisRaw("VerticalSmall") == 0f));
        }
    }

    public IEnumerator SnapIn(GameObject obj) {
        while (obj.transform.position.x != transform.position.x) {
            if (CheckAxis() && (obj.transform.position.x == transform.position.x)) {
                break;
            }
            Debug.Log((Time.time - startTime));
            float distCovered = (Time.time - startTime) * 0.5f;
            float fracJourney = Mathf.Abs(distCovered / (originalPosition.x - transform.position.x));
            Debug.Log(fracJourney);
            obj.transform.position = Vector3.Lerp(obj.transform.position, transform.position, fracJourney);
            yield return new WaitForEndOfFrame();
        }
    }
	

}

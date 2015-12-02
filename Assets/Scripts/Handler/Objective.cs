using UnityEngine;
using System.Collections;
using TwoTogether.Character;

public class Objective : MonoBehaviour {

	public GameObject handler;
	public GameObject lookingFor;

	private ObjectiveHandler objectiveHandler;
	private Color originalColor;
	private string lookingForTag;
	private SpriteRenderer spriteRenderer;
    private CharacterType lookingForType;

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
            //otherObject.transform.position = transform.position;
			objectiveHandler.SetObjectiveActivated(lookingForType, true);
		}
	}

	void OnTriggerExit2D(Collider2D otherObject) {
		if (otherObject.tag == lookingForTag) {
			spriteRenderer.color = originalColor;
			objectiveHandler.SetObjectiveActivated(lookingForType, false);
		}
	}
	

}

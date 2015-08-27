using UnityEngine;
using System.Collections;

// Allows easy access to needed Transform data
public class TransformData : MonoBehaviour {

	public string type;

    // Gets the position of the Transform
	public Vector3 GetPosition() {
		return GetComponent<Transform>().position;
	}

    // Gets the scale of the Transform
	public Vector3 GetScale() {
		return GetComponent<Transform>().lossyScale;
	}

    // Gets the type of terrain object the GameObject is
	public string GetType() {
		return type.ToUpper();
	}
}

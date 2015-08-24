using UnityEngine;
using System.Collections;

public class TransformData : MonoBehaviour {

	public string type;

	public Vector3 GetPosition() {
		return GetComponent<Transform>().position;
	}

	public Vector3 GetScale() {
		return GetComponent<Transform>().lossyScale;
	}

	public string GetType() {
		return type.ToUpper();
	}
}

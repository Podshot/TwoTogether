using UnityEngine;
using System.Collections;

// No longer used
public class DebugLevel : MonoBehaviour {
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.L)) {
			Debug.Log("Loading scene...");
			StartCoroutine("load");
		}
	}

	IEnumerator load() {
		AsyncOperation aysnc = Application.LoadLevelAsync("Level_1");
		Debug.Log(aysnc.progress);
		yield return aysnc;
		Debug.Log("Loaded scene");
	}
}

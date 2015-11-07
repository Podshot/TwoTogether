using UnityEngine;
using System.Collections;

public class GotoScene : MonoBehaviour {

	public void Goto(string scene) {
        Application.LoadLevel(scene);
    }
}

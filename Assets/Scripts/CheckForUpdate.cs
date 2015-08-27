using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Checks for standalone updates
public class CheckForUpdate : MonoBehaviour {

	public string version;
	public Text versionText;

	private Text[] texts;
	private Button updateButton;
	private string url = "http://podshot.github.io/TwoTogether/version.txt";

	void Awake() {
		texts = GetComponentsInChildren<Text>();
		updateButton = GetComponentInChildren<Button>();
		versionText.text += version;
	}
	
	IEnumerator Start () {
		WWW www = new WWW(url);
		yield return www;
		foreach (Text text in texts) {
			text.enabled = !(www.text.Equals(version));
		}
		updateButton.enabled = !(www.text.Equals(version));
		updateButton.image.enabled = !(www.text.Equals(version));
	}

	void OnMouseDown() {
		Debug.Log("Clicked update text!");
	}
}

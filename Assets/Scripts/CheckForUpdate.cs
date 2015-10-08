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
        Debug.Log("Persistent: " + Application.persistentDataPath);
        Debug.Log("Normal: " + Application.dataPath);
	}
	
	IEnumerator Start () {
		WWW www = new WWW(url);
		yield return www;
		if (www.text.Length>0 && !www.text.Equals(version)) {
			foreach (Text text in texts) {
				text.enabled = true;
			}
			updateButton.enabled = true;
			updateButton.image.enabled = true;
		}
	}

	void OnMouseDown() {
		Debug.Log("Clicked update text!");
	}
}

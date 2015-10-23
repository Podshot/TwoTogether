using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Checks for standalone updates
public class CheckForUpdate : MonoBehaviour {


    private Text txt;
    private string version;
	private string url = "http://podshot.github.io/TwoTogether/version.txt";

	void Awake() {
        txt = GetComponent<Text>();
        version = txt.text.Replace("Version: ", "");
        StartCoroutine(Check());
	}
	
	IEnumerator Check() {
		WWW www = new WWW(url);
		yield return www;
		if (www.text.Length>0 && !www.text.Equals(version)) {
            txt.color = new Color(0.74902f, 0.06275f, 0.06275f);
            GetComponent<Button>().enabled = true;
		}
        yield break;
	}
}

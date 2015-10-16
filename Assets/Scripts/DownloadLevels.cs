using UnityEngine;
using System.Collections;
using System.IO;

public class DownloadLevels : MonoBehaviour {

    public const string BASEURL = "http://podshot.github.io/TwoTogether/Levels/";
    //public const string BASEURL = "http://127.0.0.1:8000/";

    void Start() {
        StartCoroutine(Download());
    }

    public IEnumerator Download() {
        WWW manifest = new WWW(BASEURL + "levels.manifest");
        yield return manifest;
        JSONObject json = new JSONObject(manifest.text);
        System.Collections.Generic.List<JSONObject> levels = json.list;
        for (int i = 0; i < levels.Count; i++) {
            bool shouldDownload = false;
            if (File.Exists(Application.dataPath + "/Levels/" + levels[i]["Name"].str)) {
                if (!json[i]["Hash"].str.Equals(LevelManagementUtils.Hash(File.ReadAllText(Application.dataPath + "/Levels/" + levels[i]["Name"].str)))) {
                    Debug.Log("File hashes do not match for level \"" + levels[i]["Name"].str + "\"");
                    File.Delete(Application.dataPath + "/Levels/" + levels[i]["Name"].str);
                    shouldDownload = true;
                } else {
                    Debug.Log("Level \"" + levels[i]["Name"].str + "\" already exists and matches Hash");
                }
            } else {
                shouldDownload = true;
            }

            if (shouldDownload) {
                WWW level = new WWW(levels[i]["URL"].str);
                while (!level.isDone) {
                    yield return null;
                }
                if (!Directory.Exists(Application.dataPath + "/Levels/")) {
                    Directory.CreateDirectory(Application.dataPath + "/Levels/");
                }

                File.WriteAllBytes(Application.dataPath + "/Levels/" + levels[i]["Name"].str, level.bytes);

                if (!json[i]["Hash"].str.Equals(LevelManagementUtils.Hash(File.ReadAllText(Application.dataPath + "/Levels/" + levels[i]["Name"].str)))) {
                    Debug.Log("File hashes do not match for level \"" + levels[i]["Name"].str + "\"");
                }
            }
        }
        yield break;
    }
}

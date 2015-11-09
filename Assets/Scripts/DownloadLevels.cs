using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class DownloadLevels : MonoBehaviour {

    public const string BASEURL = "http://podshot.github.io/TwoTogether/Levels/";
    //public const string BASEURL = "http://127.0.0.1:8000/";

    void Awake() {
        if (!File.Exists(Application.dataPath + "/data.json")) {
            JSONObject data = new JSONObject(JSONObject.Type.OBJECT);
            data.AddField("Progress", 0);
            TextWriter writer = new StreamWriter(Application.dataPath + "/data.json");
            writer.WriteLine(data.ToString());
        }
    }

    void Start() {
        StartCoroutine(DownloadLevelFiles());
    }

    public IEnumerator DownloadLevelFiles() {
        WWW manifest = new WWW(BASEURL + "levels.manifest");
        yield return manifest;
        if (manifest.text.Length < 0 || manifest.text.Equals("")) {
            yield break;
        }
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

            if (shouldDownload || !File.Exists(Application.dataPath + "/Level_Thumbnails/" + json[i]["Thumbnail"]["Name"].str)) {
                WWW thumb = new WWW(json[i]["Thumbnail"]["URL"].str);
                while (!thumb.isDone) {
                    yield return null;
                }
                if (!Directory.Exists(Application.dataPath + "/Level_Thumbnails/")) {
                    Directory.CreateDirectory(Application.dataPath + "/Level_Thumbnails/");
                }

                File.WriteAllBytes(Application.dataPath + "/Level_Thumbnails/" + json[i]["Thumbnail"]["Name"].str, thumb.bytes);
                Debug.Log("Downloaded thumnail for " + json[i]["Name"].str);
            }
        }
        yield break;
    }
}

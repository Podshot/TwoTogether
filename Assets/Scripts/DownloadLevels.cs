using UnityEngine;
using System.Collections;
using System.IO;
//using System;
using UnityEngine.UI;

public class DownloadLevels : MonoBehaviour {

    public const string BASEURL = "http://podshot.github.io/TwoTogether/Levels/";
    //public const string BASEURL = "http://127.0.0.1:8000/";

    private GameObject startGameButton;
    private GameObject selectLevelButton;

    void Awake() {
        ConfigHandler.LoadConfig();
    }

    void Start() {
        DontDestroyOnLoad(gameObject);
        startGameButton = GameObject.Find("StartGameButton");
        selectLevelButton = GameObject.Find("SelectLevel");
        startGameButton.GetComponent<Button>().interactable = false;
        selectLevelButton.GetComponent<Button>().interactable = false;
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
            if (i > 1) {
                startGameButton.GetComponent<Button>().interactable = true;
                selectLevelButton.GetComponent<Button>().interactable = true;
            }
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
        Destroy(gameObject);
        //yield break;
    }
}

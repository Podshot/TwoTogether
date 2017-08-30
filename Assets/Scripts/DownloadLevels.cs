using UnityEngine;
using System.Collections;
using System.IO;
//using System;
using UnityEngine.UI;
using System;

public class DownloadLevels : MonoBehaviour {

    public const string BASEURL = "http://podshot.github.io/TwoTogether/Levels/";
    public GameObject[] Levels { get { return levels; } }
    public Texture2D[] Thumbnails { get { return thumbnails; } }
    //public const string BASEURL = "http://127.0.0.1:8000/";
    public bool IsInitialized { get { return initialized; } }

    private static DownloadLevels instance;
    private GameObject startGameButton;
    private GameObject selectLevelButton;
    private GameObject[] levels;
    private Texture2D[] thumbnails;
    private bool initialized = false;

    void Awake() {
        ConfigHandler.LoadConfig();
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        DontDestroyOnLoad(gameObject);
        startGameButton = GameObject.Find("StartGameButton");
        selectLevelButton = GameObject.Find("SelectLevel");
        startGameButton.GetComponent<Button>().interactable = false;
        selectLevelButton.GetComponent<Button>().interactable = false;
        if (!Directory.Exists(Path.Combine(Application.dataPath, "Level_Cache"))) {
            Directory.CreateDirectory(Path.Combine(Application.dataPath, "Level_Cache"));
        }
        if (!initialized) {
            StartCoroutine(DownloadLevelFiles());
        }
    }

    public bool HasInternetConnection() {
        NetworkReachability reachability = Application.internetReachability;
        if (reachability == NetworkReachability.NotReachable) {
            return false;
        } else {
            return true;
        }
    }

    public void CacheOldManifest(JSONObject manifest) {
        StreamWriter writer = new StreamWriter(Path.Combine(Application.dataPath, "level.manifest.cache"));
        writer.WriteLine(manifest.ToString());
        writer.Close();
    }

    public string GetOldManifest() {
        return File.ReadAllText(Path.Combine(Application.dataPath, "level.manifest.cache"));
    }

    public IEnumerator DownloadLevelFiles() {
        WWW manifest = new WWW(BASEURL + "levels.manifest");
        yield return manifest;
        string data;
        if (manifest.text.Length < 0 || manifest.text.Equals("") || !HasInternetConnection()) {
            startGameButton.GetComponent<Button>().interactable = true;
            selectLevelButton.GetComponent<Button>().interactable = true;
            data = GetOldManifest();
        } else {
            data = manifest.text;
        }
        JSONObject json = new JSONObject(data);
        CacheOldManifest(json);
        levels = new GameObject[json["Levels"].list.Count];
        thumbnails = new Texture2D[json["Levels"].list.Count];
        for (int i = 0; i < json["Levels"].list.Count; i++) {
            if (i >= 1) {
                startGameButton.GetComponent<Button>().interactable = true;
                selectLevelButton.GetComponent<Button>().interactable = true;
            }
            AssetBundle bundle;
            WWW www;
            JSONObject level = json["Levels"][i];
            if (HasInternetConnection()) {
                www = new WWW(level["URL"].str);
                yield return www;
                File.WriteAllBytes(Path.Combine(Path.Combine(Application.dataPath, "Level_Cache"), level["Name"].str), www.bytes);
                bundle = www.assetBundle;
                www.Dispose();
            } else {
                bundle = AssetBundle.LoadFromFile(Path.Combine(Path.Combine(Application.dataPath, "Level_Cache"), level["Name"].str));
            }
            GameObject go = bundle.LoadAsset<GameObject>("Level_" + (i + 1));
            int result = 0;
            int.TryParse(go.name.Replace("Level_", ""), out result);
            levels[result - 1] = go;

            Texture2D tex = bundle.LoadAsset<Texture2D>("Level_" + (i + 1) + "_Thumbnail");
            thumbnails[result - 1] = tex;
            bundle.Unload(false);
        }
        initialized = true;
    }
}

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class BuildLevelManifest {

    [MenuItem("Tools/Test")]
    public static void Test() {
        string[] levels = Directory.GetFiles(Application.dataPath + "/Levels_Exported/", "*.json");
        int i = 0;
        Debug.Log(Application.dataPath + "/Level_Thumbnails/" + levels[i].Replace(Application.dataPath + "/Levels_Exported/", "").Replace(".json", ".png"));
    }

    [MenuItem("Tools/Format JSON")]
    public static void FormatJSON() {
        string[] files = Directory.GetFiles(Application.dataPath + "/Levels_Exported/", "*.json");
        for (int i = 0; i < files.Length; i++) {
            string content = File.ReadAllText(files[i]);
            JSONObject data = new JSONObject(content);
            File.Move(files[i], files[i].Replace(".json", ".raw_json"));
            TextWriter writer = new StreamWriter(files[i]);
            writer.Write(data.ToString());
            writer.Close();
        }
    }

    [MenuItem("Tools/Build Level Manifest")]
	public static void BuildManifest() {
        JSONObject json = new JSONObject(JSONObject.Type.ARRAY);
        string[] levels = Directory.GetFiles(Application.dataPath + "/Levels_Exported/", "*.json");

        for (int i = 0; i < levels.Length; i++) {
            JSONObject data = new JSONObject(JSONObject.Type.OBJECT);
            string fileContents = File.ReadAllText(levels[i]);
            string hash = LevelManagementUtils.Hash(fileContents);
            data.AddField("Hash", hash);
            string url = levels[i].Replace(Application.dataPath, "http://podshot.github.io/TwoTogether" /*"http://127.0.0.1:8000/"*/);
            url = url.Replace("_Exported", "");
            data.AddField("URL", url);
            data.AddField("Name", levels[i].Replace(Application.dataPath + "/Levels_Exported/", ""));

            JSONObject thumb = new JSONObject(JSONObject.Type.OBJECT);
            byte[] thumbnail = File.ReadAllBytes(Application.dataPath + "/Level_Thumbnails/" + levels[i].Replace(Application.dataPath + "/Levels_Exported/", "").Replace(".json", ".png"));
            string h = LevelManagementUtils.HashFromBytes(thumbnail);
            thumb.AddField("URL", "http://podshot.github.io/TwoTogether/Thumbnails/" + levels[i].Replace(Application.dataPath + "/Levels_Exported/", "").Replace(".json", ".png"));
            thumb.AddField("Name", levels[i].Replace(Application.dataPath + "/Levels_Exported/", "").Replace(".json", ".png"));

            data.AddField("Thumbnail", thumb);
            json.Add(data);
        }

        TextWriter writer = new StreamWriter(Application.dataPath + "/Levels_Exported/levels_.manifest");
        writer.WriteLine(json.ToString(true));
        writer.Close();
        Debug.Log("Wrote manifest file");
    }
}

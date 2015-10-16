using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class BuildLevelManifest {

    [MenuItem("Tools/Build Level Manifest")]
	public static void Build() {
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

            json.Add(data);
        }
        TextWriter writer = new StreamWriter(Application.dataPath + "/Levels_Exported/levels.manifest");
        writer.WriteLine(json.ToString(true));
        writer.Close();
        Debug.Log("Wrote manifest file");
    }
}

//#define BUILDING

#if !BUILDING
using UnityEngine;
using System.Collections;
//using System.IO;
using System.Collections.Generic;
using System.IO;

public class ConfigHandler : MonoBehaviour {

    private static string CONFIG_PATH { get { return Application.dataPath + "/data.json"; } }
    private static JSONObject data;
    public static JSONObject Config { get { return data; } }
    private static bool loaded = false;
    public static bool IsLoaded { get { return loaded; } }
    private static string defaults = "{\"Progress\": -1}";

    public static JSONObject LoadConfig() {
        if (!File.Exists(CONFIG_PATH)) {
            data = new JSONObject(defaults);
            Debug.Log(data["Progress"].n);
            SaveConfig();
            loaded = true;
        } else {
            string _data = File.ReadAllText(CONFIG_PATH);
            data = new JSONObject(_data);
            loaded = true;
        }
        return data;
    }

    public static void SaveConfig() {
        TextWriter writer = new StreamWriter(Application.dataPath + "/data.json");
        writer.WriteLine(data.ToString());
        writer.Close();
    }
}
#endif

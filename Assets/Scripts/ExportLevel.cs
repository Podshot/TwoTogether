using UnityEngine;
using System.IO;
using System.Collections;

public class ExportLevel : MonoBehaviour {
    public GameObject terrainParent;
    private TextWriter writer;

    void Start() {
        writer = new StreamWriter("level_exported.json");
    }

    void Update() {
        if (/*(Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftCommand)) &&*/ Input.GetKeyDown(KeyCode.E)) {
            Export();
        }
    }

    void Export() {
        Debug.Log("Exporting...");
        JSONObject level = new JSONObject(JSONObject.Type.OBJECT);

        JSONObject normalTerrain = new JSONObject(JSONObject.Type.ARRAY);
        JSONObject shadowTerrain = new JSONObject(JSONObject.Type.ARRAY);

        level.AddField("NormalTerrain", normalTerrain);
        level.AddField("ShadowTerrain", shadowTerrain);

        JSONObject addTo = null;

        foreach (SpriteRenderer render in terrainParent.GetComponentsInChildren<SpriteRenderer>()) {

            switch (render.transform.parent.name.ToLower()) {
                case "terrain":
                    addTo = normalTerrain;
                    break;
                case "backgroundshadows":
                    addTo = shadowTerrain;
                    break;
                default:
                    break;
            }
            Transform trans = render.transform;

            JSONObject to = new JSONObject(JSONObject.Type.OBJECT);
            JSONObject scale = new JSONObject(JSONObject.Type.ARRAY);
            JSONObject pos = new JSONObject(JSONObject.Type.ARRAY);

            to.AddField("Scale", scale);
            to.AddField("Position", pos);
            to.AddField("Color", render.color.ToHexStringRGBA());
            to.AddField("Name", trans.name);

            scale.Add(trans.lossyScale.x);
            scale.Add(trans.lossyScale.y);
            scale.Add(trans.lossyScale.z);

            pos.Add(trans.position.x);
            pos.Add(trans.position.y);
            pos.Add(trans.position.z);

            addTo.Add(to);
        }
        writer.WriteLine(level.ToString());
        writer.Close();
        Debug.Log("Exported");
    }
}

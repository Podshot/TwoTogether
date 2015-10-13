using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Collections;

public class ExportLevel : MonoBehaviour {

    public GameObject terrainParent;
    public GameObject objectivesParent;
    public GameObject spawnpointsParent;
    public GameObject specialParent;
    public Text helpText;

    private TextWriter writer;

    #if UNITY_EDITOR
    void Update() {
        if (/*(Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftCommand)) &&*/ Input.GetKeyDown(KeyCode.E)) {
            Export();
        }
    }
    #endif

    private JSONObject GetTransformData(Transform trans, Color color) {
        JSONObject obj = GetTransformData(trans);
        obj.AddField("Color", color.ToHexStringRGBA());
        return obj;
    }

    private JSONObject GetTransformData(Transform trans) {
        JSONObject obj = new JSONObject(JSONObject.Type.OBJECT);
        JSONObject scale = new JSONObject(JSONObject.Type.ARRAY);
        JSONObject pos = new JSONObject(JSONObject.Type.ARRAY);

        obj.AddField("Scale", scale);
        obj.AddField("Position", pos);

        // Debug field, can be removed when export testing is done
        obj.AddField("Name", trans.name);

        scale.Add(trans.lossyScale.x);
        scale.Add(trans.lossyScale.y);
        scale.Add(trans.lossyScale.z);

        pos.Add(trans.position.x);
        pos.Add(trans.position.y);
        pos.Add(trans.position.z);
        return obj;
    }

    void Export() {
        Debug.Log("Exporting...");
        writer = new StreamWriter(Application.dataPath + "/Levels_Exported/" + Application.loadedLevelName + ".json");

        JSONObject level = new JSONObject(JSONObject.Type.OBJECT);

        JSONObject normalTerrain = new JSONObject(JSONObject.Type.ARRAY);
        JSONObject shadowTerrain = new JSONObject(JSONObject.Type.ARRAY);
        JSONObject special = new JSONObject(JSONObject.Type.ARRAY);
        JSONObject text = new JSONObject(JSONObject.Type.OBJECT);

        JSONObject objectives = new JSONObject(JSONObject.Type.OBJECT);
        JSONObject spawnpoints = new JSONObject(JSONObject.Type.OBJECT);

        level.AddField("Current ID", Application.loadedLevelName);
        level.AddField("Next ID", "<add manually>");
        level.AddField("Text", text);
        level.AddField("NormalTerrain", normalTerrain);
        level.AddField("ShadowTerrain", shadowTerrain);
        level.AddField("Special", special);
        level.AddField("Objectives", objectives);
        level.AddField("Spawnpoints", spawnpoints);

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
            JSONObject to = GetTransformData(render.transform, render.color);
            addTo.Add(to);
        }

        foreach (SpriteRenderer renderer in objectivesParent.GetComponentsInChildren<SpriteRenderer>()) {
            JSONObject obj = GetTransformData(renderer.transform, renderer.color);
            switch (renderer.transform.name.ToLower()) {
                case "bigcubeobjective":
                    objectives.AddField("Blue", obj);
                    break;
                case "smallcubeobjective":
                    objectives.AddField("Red", obj);
                    break;
                default:
                    break;
            }
        }

        foreach (Transform trans in spawnpointsParent.GetComponentsInChildren<Transform>()) {
            JSONObject point = GetTransformData(trans);
            switch (trans.name.ToLower()) {
                case "smallcubespawnpoint":
                    spawnpoints.AddField("Red", point);
                    break;
                case "bigcubespawnpoint":
                    spawnpoints.AddField("Blue", point);
                    break;
                default:
                    break;
            }
        }
        
        if (specialParent != null) {
            foreach (Transform trans in specialParent.GetComponentsInChildren<Transform>()) {
                if (trans.name.Equals("KillerBlocks")) {
                    foreach (SpriteRenderer renderer in trans.GetComponentsInChildren<SpriteRenderer>()) {
                        JSONObject obj = GetTransformData(renderer.transform, renderer.color);
                        obj.AddField("Type", "KillerTerrain");
                        obj.AddField("Target", renderer.GetComponent<KillerTerrain>().GetTargetTag());
                        special.Add(obj);
                    }
                }
            }
        }

        // Start Help Text serialization
        JSONObject dimensions = new JSONObject(JSONObject.Type.ARRAY);

        dimensions.Add(helpText.rectTransform.rect.width);
        dimensions.Add(helpText.rectTransform.rect.height);

        text.AddField("Dimensions", dimensions);
        text.AddField("Text", helpText.text);
        text.AddField("Color", helpText.color.ToHexStringRGBA());

        writer.WriteLine(level.ToString(true));
        writer.Close();
        Debug.Log("Exported");
    }
}
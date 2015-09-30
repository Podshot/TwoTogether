using UnityEngine;
using System.IO;
using System.Collections;

public class ExportLevel : MonoBehaviour {

    public GameObject terrainParent;
    public GameObject objectivesParent;
    public GameObject spawnpointsParent;

    private TextWriter writer;

    void Start() {
        writer = new StreamWriter(Application.loadedLevelName + ".json");
    }

    #if UNITY_EDITOR
    void Update() {
        if (/*(Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftCommand)) &&*/ Input.GetKeyDown(KeyCode.E)) {
            Export();
        }
    }
    #endif

    void Export() {
        Debug.Log("Exporting...");

        JSONObject level = new JSONObject(JSONObject.Type.OBJECT);

        JSONObject normalTerrain = new JSONObject(JSONObject.Type.ARRAY);
        JSONObject shadowTerrain = new JSONObject(JSONObject.Type.ARRAY);

        JSONObject objectives = new JSONObject(JSONObject.Type.OBJECT);
        JSONObject spawnpoints = new JSONObject(JSONObject.Type.OBJECT);

        level.AddField("ID", Application.loadedLevelName);
        level.AddField("NormalTerrain", normalTerrain);
        level.AddField("ShadowTerrain", shadowTerrain);
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
            Transform trans = render.transform;

            JSONObject to = new JSONObject(JSONObject.Type.OBJECT);
            JSONObject scale = new JSONObject(JSONObject.Type.ARRAY);
            JSONObject pos = new JSONObject(JSONObject.Type.ARRAY);

            to.AddField("Scale", scale);
            to.AddField("Position", pos);
            to.AddField("Color", render.color.ToHexStringRGBA());

            // Debug field, can be removed when export testing is done
            to.AddField("Name", trans.name);

            scale.Add(trans.lossyScale.x);
            scale.Add(trans.lossyScale.y);
            scale.Add(trans.lossyScale.z);

            pos.Add(trans.position.x);
            pos.Add(trans.position.y);
            pos.Add(trans.position.z);

            addTo.Add(to);
        }

        foreach (SpriteRenderer renderer in objectivesParent.GetComponentsInChildren<SpriteRenderer>()) {
            JSONObject obj = new JSONObject(JSONObject.Type.OBJECT);
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
            Transform trans = renderer.transform;

            JSONObject scale = new JSONObject(JSONObject.Type.ARRAY);
            JSONObject pos = new JSONObject(JSONObject.Type.ARRAY);

            obj.AddField("Scale", scale);
            obj.AddField("Position", pos);
            obj.AddField("Color", renderer.color.ToHexStringRGBA());

            // Debug field, can be removed when export testing is done
            obj.AddField("Name", trans.name);

            scale.Add(trans.lossyScale.x);
            scale.Add(trans.lossyScale.y);
            scale.Add(trans.lossyScale.z);

            pos.Add(trans.position.x);
            pos.Add(trans.position.y);
            pos.Add(trans.position.z);
        }

        foreach (Transform trans in spawnpointsParent.GetComponentsInChildren<Transform>()) {
            JSONObject point = new JSONObject(JSONObject.Type.OBJECT);
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
            JSONObject scale = new JSONObject(JSONObject.Type.ARRAY);
            JSONObject pos = new JSONObject(JSONObject.Type.ARRAY);

            point.AddField("Scale", scale);
            point.AddField("Position", pos);

            // Debug field, can be removed when export testing is done
            point.AddField("Name", trans.name);

            scale.Add(trans.lossyScale.x);
            scale.Add(trans.lossyScale.y);
            scale.Add(trans.lossyScale.z);

            pos.Add(trans.position.x);
            pos.Add(trans.position.y);
            pos.Add(trans.position.z);
        }
        writer.WriteLine(level.ToString(true));
        writer.Close();
        Debug.Log("Exported");
    }
}
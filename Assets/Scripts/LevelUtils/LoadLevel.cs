using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class LoadLevel : MonoBehaviour {

    public Text helpText;
    public GameObject terrainParent;
    public GameObject objectivesParent;
    public GameObject shadowParent;
    public GameObject spawnpointsParent;
    public GameObject normalTerrainPrefab;
    public GameObject shadowTerrainPrefab;
    public GameObject redKillerPrefab;
    public GameObject blueKillerPrefab;

    private GameState gameState;

	// Use this for initialization
	void Awake() {
        LoadLevelData("level_1");
	}

    void Start() {
        gameState = Camera.main.GetComponent<GameState>();
        gameState.SetHelpText(helpText);
        gameState.SetParents(terrainParent, objectivesParent, spawnpointsParent);
        gameState.GiveLoadLevelInstance(this);
        gameState.Ready();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.B)) {
            RemoveOldLevel();
        }
    }

    private GameObject AddTerrainPiece(GameObject prefab, GameObject parent, JSONObject data) {
        GameObject gobj = Instantiate(prefab, new Vector3(data["Position"][0].n, data["Position"][1].n, data["Position"][2].n), Quaternion.identity) as GameObject;
        gobj.transform.localScale = new Vector3(data["Scale"][0].n, data["Scale"][1].n, data["Scale"][2].n);
        gobj.transform.parent = parent.transform;
        Color outColor;
        Color.TryParseHexString(data["Color"].str, out outColor);
        gobj.GetComponent<SpriteRenderer>().color = outColor;
        return gobj;
    }

    public void LoadLevelData(string v) {
        string levelData = System.IO.File.ReadAllText(Application.dataPath + "/Levels/" + v + ".json");
        JSONObject level = new JSONObject(levelData);

        foreach (Transform trans in spawnpointsParent.GetComponentsInChildren<Transform>()) {
            if (trans.name.Equals("SmallCubeSpawnpoint")) {
                trans.position = new Vector3(level["Spawnpoints"]["Red"]["Position"][0].n, level["Spawnpoints"]["Red"]["Position"][1].n, level["Spawnpoints"]["Red"]["Position"][2].n);
            }
            if (trans.name.Equals("BigCubeSpawnpoint")) {
                trans.position = new Vector3(level["Spawnpoints"]["Blue"]["Position"][0].n, level["Spawnpoints"]["Blue"]["Position"][1].n, level["Spawnpoints"]["Blue"]["Position"][2].n);
            }
        }

        foreach (Transform trans in objectivesParent.GetComponentsInChildren<Transform>()) {
            if (trans.name.Equals("BigCubeObjective")) {
                trans.position = new Vector3(level["Objectives"]["Blue"]["Position"][0].n, level["Objectives"]["Blue"]["Position"][1].n, level["Objectives"]["Blue"]["Position"][2].n);
            }
            if (trans.name.Equals("SmallCubeObjective")) {
                trans.position = new Vector3(level["Objectives"]["Red"]["Position"][0].n, level["Objectives"]["Red"]["Position"][1].n, level["Objectives"]["Red"]["Position"][2].n);
            }
        }


        foreach (JSONObject obj in level["NormalTerrain"].list) {
            AddTerrainPiece(normalTerrainPrefab, terrainParent, obj);
        }

        foreach (JSONObject obj in level["ShadowTerrain"].list) {
            AddTerrainPiece(shadowTerrainPrefab, shadowParent, obj);
        }

        helpText.text = level["Text"]["Text"].str;
        helpText.rectTransform.sizeDelta = new Vector2(level["Text"]["Dimensions"][0].n, level["Text"]["Dimensions"][1].n);
    }

    public void RemoveOldLevel() {
        SpriteRenderer[] terrain = terrainParent.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer renderer in terrain) {
            Destroy(renderer.gameObject);
        }
        helpText.text = "";
    }
}

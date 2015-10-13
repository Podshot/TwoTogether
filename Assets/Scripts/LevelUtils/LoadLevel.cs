using UnityEngine;
using UnityEngine.UI;
using LevelExceptions;
using System.Collections;
using System;

public class LoadLevel : MonoBehaviour {

    public const float mapFormat = 1f;

    public Text helpText;
    public GameObject terrainParent;
    public GameObject objectivesParent;
    public GameObject shadowParent;
    public GameObject spawnpointsParent;
    public GameObject specialParent;
    public GameObject normalTerrainPrefab;
    public GameObject shadowTerrainPrefab;
    public GameObject redKillerPrefab;
    public GameObject blueKillerPrefab;

    private string nextLevel;
    private GameState gameState;
    private delegate void PlaceSpecials(JSONObject specialsObj);
    private PlaceSpecials ParseSpecials;

	// Use this for initialization
	void Awake() {
        ParseSpecials += ParseKillerBlocks;

        LoadLevelData("level_3");
	}

    void Start() {
        gameState = Camera.main.GetComponent<GameState>();
        gameState.SetHelpText(helpText);
        gameState.SetParents(terrainParent, objectivesParent, spawnpointsParent, specialParent);
        gameState.GiveLoadLevelInstance(this);
        gameState.Ready();
    }

    public GameObject AddTerrainPiece(GameObject prefab, GameObject parent, JSONObject data) {
        GameObject gobj = Instantiate(prefab, new Vector3(data["Position"][0].n, data["Position"][1].n, data["Position"][2].n), Quaternion.identity) as GameObject;
        gobj.transform.localScale = new Vector3(data["Scale"][0].n, data["Scale"][1].n, data["Scale"][2].n);
        gobj.transform.parent = parent.transform;
        Color outColor;
        Color.TryParseHexString(data["Color"].str, out outColor);
        gobj.GetComponent<SpriteRenderer>().color = outColor;
        return gobj;
    }

    public void LoadLevelData(string v) {
        string levelData = System.IO.File.ReadAllText(Application.dataPath + "/Levels_Exported/" + v + ".json");
        JSONObject level = new JSONObject(levelData);

        if (level["Map Format"].n > mapFormat) {
            throw new UnsupportedLevelVersion("Map Format not supported yet!");
        }

        nextLevel = level["Next ID"].str;

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

        // Special Terrain creation
        ParseSpecials(level["Special"]);

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

    public string GetNextID() {
        return nextLevel;
    }

    private void ParseKillerBlocks(JSONObject specials) {
        foreach (JSONObject obj in specials.list) {
            switch (obj["Type"].str) {
                case "KillerTerrain":
                    if (obj["Target"].str.Equals("BigCube")) {
                        AddTerrainPiece(blueKillerPrefab, specialParent, obj);
                    } else if (obj["Target"].str.Equals("SmallCube")) {
                        // Not actually used in a level yet, so supply nulls so an error will show
                        AddTerrainPiece(null, null, obj);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}

using UnityEngine;
using System.Collections;
using System;

public class LoadLevel : MonoBehaviour {

    public GameObject terrainParent;
    public GameObject objectiveParent;
    public GameObject shadowParent;
    public GameObject spawnpointsParent;
    public GameObject normalTerrainPrefab;
    public GameObject shadowTerrainPrefab;
    public GameObject redKillerPrefab;
    public GameObject blueKillerPrefab;

	// Use this for initialization
	void Start () {
        Debug.Log(Application.dataPath + "/Levels/" + "Level_1");
        LoadLevelData("level_1");
	}

    private void LoadLevelData(string v) {
        string levelData = System.IO.File.ReadAllText(Application.dataPath + "/Levels/" + v + ".json");
        JSONObject level = new JSONObject(levelData);
        Debug.Log(level["ID"].str);
    }

    private void RemoveOldLevel() {

    }
}

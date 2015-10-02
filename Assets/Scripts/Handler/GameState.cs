using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class GameState : MonoBehaviour {

    private Text helpText;
    private GameObject spawnpointsParent;
    private LoadLevel instance;
    private ObjectiveHandler objectiveHandler;
    private TerrainFader terrainFader;
    private List<Controller> controllers;


    public void SetHelpText(Text txt) {
        helpText = txt;
    }

    public Text GetHelpText() {
        return helpText;
    }

    public void Ready() {
        objectiveHandler.Load();
        terrainFader.Load();
        spawnpointsParent.GetComponent<SpawnHandler>().Load();

        controllers = spawnpointsParent.GetComponent<SpawnHandler>().GetControllers();
        objectiveHandler.FadeIn();
        StartCoroutine(terrainFader.FadeIn());
        foreach (Controller controller in controllers) {
            controller.FadeIn();
        }
    }

    public void SetParents(GameObject terrain, GameObject objectives, GameObject spawnpoints) {
        terrainFader = terrain.GetComponent<TerrainFader>();
        objectiveHandler = objectives.GetComponent<ObjectiveHandler>();
        spawnpointsParent = spawnpoints;
    }

    public void GiveLoadLevelInstance(LoadLevel loadLevel) {
        instance = loadLevel;
    }

    public /*IEnumerator*/ void _Switch() {
        Debug.LogError("Switch() called");
        helpText.CrossFadeAlpha(0f, 0.5f, false);
        StartCoroutine(terrainFader.FadeOut());
        objectiveHandler.FadeOut();
        foreach (Controller controller in controllers) {
            controller.FadeOut();
        }
        while (!terrainFader.GetFadeOut()) { Debug.Log(terrainFader.GetFadeOut()); }
        terrainFader.Cleanup();
        //yield return new WaitForSeconds(2f);
        LoadLevel("level_2");
    }

    public void LoadLevel(string lvl) {
        terrainFader.Cleanup();
        instance.RemoveOldLevel();
        instance.LoadLevelData(lvl);
        Ready();
    }

    public void StopControllers() {
        foreach (Controller controller in controllers) {
            controller.GetRigidbody().velocity = new Vector2(0.125f, controller.GetRigidbody().velocity.y);
        }
    }
}

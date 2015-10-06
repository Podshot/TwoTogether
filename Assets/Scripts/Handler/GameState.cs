using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class GameState : MonoBehaviour {

    private Text helpText;
    private TextFader textFader;
    private GameObject spawnpointsParent;
    private LoadLevel instance;
    private ObjectiveHandler objectiveHandler;
    private TerrainFader terrainFader;
    private List<Controller> controllers;

    public void SetHelpText(Text txt) {
        helpText = txt;
        textFader = txt.GetComponent<TextFader>();
    }

    public Text GetHelpText() {
        return helpText;
    }

    public void Ready() {
        spawnpointsParent.GetComponent<SpawnHandler>().Load();
        if (controllers == null) {
            controllers = spawnpointsParent.GetComponent<SpawnHandler>().GetControllers();
        }

        textFader.Load();
        objectiveHandler.Load();
        terrainFader.Load();
        foreach (Controller controller in controllers) {
            controller.Load();
        }

        StartCoroutine(textFader.FadeIn());
        StartCoroutine(objectiveHandler.FadeIn());
        StartCoroutine(terrainFader.FadeIn());
        foreach (Controller controller in controllers) {
            StartCoroutine(controller.FadeIn());
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

    public IEnumerator Switch() {
        StartCoroutine(textFader.FadeOut());
        StartCoroutine(terrainFader.FadeOut());
        StartCoroutine(objectiveHandler.FadeOut());
        foreach (Controller controller in controllers) {
            StartCoroutine(controller.FadeOut());
        }
        yield return new WaitForSeconds(2f);
        LoadLevel("level_2");
    }

    public void LoadLevel(string lvl) {
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

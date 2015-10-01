using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class GameState : MonoBehaviour {

    private static Text helpText;
    private static GameObject spawnpointsParent;
    private static LoadLevel instance;
    private static ObjectiveHandler objectiveHandler;
    private static TerrainFader terrainFader;
    private static List<Controller> controllers;

    public static void SetHelpText(Text txt) {
        helpText = txt;
    }

    public static Text GetHelpText() {
        return helpText;
    }

    public static void Ready() {
        objectiveHandler.Load();
        terrainFader.Load();
        spawnpointsParent.GetComponent<SpawnHandler>().Load();

        controllers = spawnpointsParent.GetComponent<SpawnHandler>().GetControllers();
        objectiveHandler.FadeIn();
        terrainFader.FadeIn();
        foreach (Controller controller in controllers) {
            controller.FadeIn();
        }
    }

    public static void SetParents(GameObject terrain, GameObject objectives, GameObject spawnpoints) {
        terrainFader = terrain.GetComponent<TerrainFader>();
        objectiveHandler = objectives.GetComponent<ObjectiveHandler>();
        spawnpointsParent = spawnpoints;
    }

    public static void GiveLoadLevelInstance(LoadLevel loadLevel) {
        instance = loadLevel;
    }

    public static void Switch() {
        helpText.CrossFadeAlpha(0f, 0.5f, false);
        terrainFader.FadeOut();
        objectiveHandler.FadeOut();
        foreach (Controller controller in controllers) {
            controller.FadeOut();
        }
    }

    public static void StopControllers() {
        foreach (Controller controller in controllers) {
            controller.GetRigidbody().velocity = new Vector2(0.5f, controller.GetRigidbody().velocity.y);
        }
    }
}

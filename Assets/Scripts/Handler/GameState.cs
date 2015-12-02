using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using TwoTogether.LevelExceptions;
using System.IO;
using TwoTogether.Character;

public class GameState : MonoBehaviour {

    public Text levelErrorText;

    private Text helpText;
    private TextFader textFader;
    private GameObject spawnpointsParent;
    private LoadLevel instance;
    private ObjectiveHandler objectiveHandler;
    private TerrainFader terrainFader;
    private List<Controller> controllers;
    private GameObject specialParent;
    private float[] alphas;

    public void SetHelpText(Text txt) {
        helpText = txt;
        textFader = txt.GetComponent<TextFader>();
    }

    public Text GetHelpText() {
        return helpText;
    }

    /// <summary>
    /// Loads all elements of the level and fades them in.
    /// </summary>
    public void Ready() {
        alphas = new float[2];
        spawnpointsParent.GetComponent<SpawnHandler>().Load();
        if (controllers == null) {
            controllers = spawnpointsParent.GetComponent<SpawnHandler>().GetControllers();
        }

        // Should probably be refactored similarly to how specials are faded in
        //foreach (KillerTerrain kt in specialParent.GetComponentsInChildren<KillerTerrain>()) {
        //    kt.Load();
        //}
        foreach (ILoadable loadable in specialParent.GetComponentsInChildren(typeof(ILoadable))) {
            loadable.Load();
        }

        textFader.Load();
        objectiveHandler.Load();
        terrainFader.Load();
        foreach (Controller controller in controllers) {
            controller.Load();
        }

        // FadeIn() methods should be IEnumerators and be called through StartCoroutine()
        StartCoroutine(textFader.FadeIn());
        StartCoroutine(objectiveHandler.FadeIn());
        StartCoroutine(terrainFader.FadeIn());
        foreach (Controller controller in controllers) {
            StartCoroutine(controller.FadeIn());
        }
        foreach (IFadeable fadeable in specialParent.GetComponentsInChildren(typeof(IFadeable))) {
            StartCoroutine(fadeable.FadeIn());
        }
    }

    public void SetParents(GameObject terrain, GameObject objectives, GameObject spawnpoints, GameObject special) {
        terrainFader = terrain.GetComponent<TerrainFader>();
        objectiveHandler = objectives.GetComponent<ObjectiveHandler>();
        spawnpointsParent = spawnpoints;
        specialParent = special;
    }

    public void GiveLoadLevelInstance(LoadLevel loadLevel) {
        instance = loadLevel;
    }

    /// <summary>
    /// Fades out all level elements, updates the progress in "data.json" and goes onto the next level
    /// </summary>
    /// <returns></returns>
    public IEnumerator Switch() {
        StartCoroutine(textFader.FadeOut());
        StartCoroutine(terrainFader.FadeOut());
        StartCoroutine(objectiveHandler.FadeOut());
        foreach (Controller controller in controllers) {
            StartCoroutine(controller.FadeOut());
        }
        foreach (IFadeable fadeable in specialParent.GetComponentsInChildren(typeof(IFadeable))) {
            StartCoroutine(fadeable.FadeOut());
        }

        if (instance.GetNextID().Contains("level_")) {
            Debug.Log(System.Convert.ToSingle(instance.GetNextID().Replace("level_", "")));
            JSONObject progress = new JSONObject(File.ReadAllText(Application.dataPath + "/data.json"));
            progress["Progress"].n = System.Convert.ToSingle(instance.GetNextID().Replace("level_", ""));
            TextWriter writer = new StreamWriter(Application.dataPath + "/data.json");
            writer.WriteLine(progress.ToString());
            writer.Close();
        }
        yield return new WaitForSeconds(2f);
        LoadLevel(instance.GetNextID());
    }

    public void LoadLevel(string lvl) {
        instance.RemoveOldLevel();
        try {
            instance.LoadLevelData(lvl);
        } catch (UnsupportedLevelVersion e) {
            levelErrorText.text = "The rest of the levels are not supported by this version of TwoTogether.\nPlease click this text to go to the downloads page and update the game.";
            levelErrorText.gameObject.SetActive(true);
        }
        objectiveHandler.Reset();
        Ready();
    }

    public void StopControllers() {
        foreach (Controller controller in controllers) {
            controller.GetRigidbody().velocity = new Vector2(0.125f, controller.GetRigidbody().velocity.y);
        }
    }

    /// <summary>
    /// Pauses the movement of the Controllers and temporarily disables their RigidBodies
    /// </summary>
    /// <param name="pause">True if the Controllers should be pause, False to resume</param>
    public void PauseControllers(bool pause) {
        foreach (Controller controller in controllers) {
            controller.enabled = !pause;
            controller.GetRigidbody().isKinematic = pause;
        }
    }

    /// <summary>
    /// Sets all level elements to partially faded state
    /// </summary>
    /// <param name="fade">True to partially fade, False to unfade</param>
    public void PartiallyFadeCharactersAndObjectives(bool fade) {
        if (fade) {
            foreach (Controller controller in controllers) {
                SpriteRenderer renderer = controller.gameObject.GetComponent<SpriteRenderer>();
                renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0.5f);
            }
            SpriteRenderer[] renderers = { objectiveHandler.blueObjective.GetComponent<SpriteRenderer>(), objectiveHandler.redObjective.GetComponent<SpriteRenderer>() };
            for (int i = 0; i < renderers.Length; i++) {
                alphas[i] = renderers[i].color.a;
                renderers[i].color = new Color(renderers[i].color.r, renderers[i].color.g, renderers[i].color.b, 0.5f);
            }
        } else {
            foreach (Controller controller in controllers) {
                SpriteRenderer renderer = controller.gameObject.GetComponent<SpriteRenderer>();
                renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1f);
            }
            SpriteRenderer[] renderers = { objectiveHandler.blueObjective.GetComponent<SpriteRenderer>(), objectiveHandler.redObjective.GetComponent<SpriteRenderer>() };
            for (int i = 0; i < renderers.Length; i++) {
                renderers[i].color = new Color(renderers[i].color.r, renderers[i].color.g, renderers[i].color.b, alphas[i]);
            }
        }
    }
}

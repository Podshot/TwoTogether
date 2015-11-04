using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class GenerateThumbnails : MonoBehaviour {

    public GameObject prefab;
    public GameObject paren;
    public Image unknown;

    private float[][] points = new float[][] { new float[] { -200f, 125f }, new float[] { 200f, 125f }, new float[] { -200f, -145f}, new float[] { 200f, -145f } }; 

	// Use this for initialization
	void Start () {
        JSONObject progress = new JSONObject(File.ReadAllText(Application.dataPath + "/data.json"));
        Image[] images = GetComponentsInChildren<Image>();
        for (int i = 0; i < images.Length; i++) {
            if (images[i].name == "ToMainMenu") {
                continue;
            }
            if ((i + 1) <= progress["Progress"].n) {
                Image image = images[i];
                Texture2D tex2d = new Texture2D(800, 600);
                tex2d.LoadImage(File.ReadAllBytes(Application.dataPath + "/Level_Thumbnails/level_" + (i + 1) + ".png"));
                image.sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), image.sprite.pivot);
                image.enabled = true;

                Text txt = image.GetComponentInChildren<Text>();
                txt.text = "Level " + (i + 1);
                txt.enabled = false;

                LevelIdentity id = image.gameObject.AddComponent<LevelIdentity>();
                id.SetID("level_" + (i + 1));
                id.SetCanClick(true);
            } else {
                Image image = images[i];
                image.sprite = unknown.sprite;
                image.color = unknown.color;
                image.enabled = true;

                Text txt = image.GetComponentInChildren<Text>();
                txt.text = "?";
                txt.enabled = false;

                LevelIdentity id = image.gameObject.AddComponent<LevelIdentity>();
                id.SetCanClick(false);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {

	}
}

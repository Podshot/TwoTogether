using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class GenerateThumbnails : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Image[] images = GetComponentsInChildren<Image>();
        for (int i = 0; i < images.Length; i++) {
            if (i > 2) {
                continue;
            }
            Image image = images[i];
            Texture2D tex2d = new Texture2D(800, 600);
            tex2d.LoadImage(File.ReadAllBytes(Application.dataPath + "/Level_Thumbnails/level_" + (i + 1) + ".png"));
            image.sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), image.sprite.pivot);

            Text txt = image.GetComponentInChildren<Text>();
            txt.text = "Level " + (i + 1);
            txt.enabled = false;

            LevelIdentity id = image.gameObject.AddComponent<LevelIdentity>();
            id.SetID("level_" + (i + 1));
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

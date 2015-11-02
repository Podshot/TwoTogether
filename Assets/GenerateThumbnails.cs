using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class GenerateThumbnails : MonoBehaviour {

    public Image image;

	// Use this for initialization
	void Start () {
        Texture2D tex2d = new Texture2D(435, 326);
        tex2d.LoadImage(File.ReadAllBytes(Application.dataPath + "/Level_Thumbnails/level_1.png"));
        image.sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), image.sprite.pivot);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GenerateThumbnails : MonoBehaviour {

    public Text pageCounter;
    public GameObject prefab;
    public Image unknown;
    public Transform paren;
    public Sprite[] thumbnails;

    private int totalPages;
    private int currentPage = 0;
    private bool move;
    private Vector3 target;
    private const float OFFSET = 350f;
    private DownloadLevels levelDownloader;

    void Awake() {
        //GameObject loader = GameObject.Find("LevelDownloader");
        //if (loader == null) {
        //    Application.LoadLevel("MainMenu");
        //}
    }

    void Start () {
        Image[] images = GetComponentsInChildren<Image>();
        //int numberOfLevels = levelDownloader.Levels.Length;
        int numberOfLevels = SceneManager.sceneCount;
        int remainder;
        int result = Math.DivRem(numberOfLevels, 4, out remainder);
        if (numberOfLevels < 4) {
            totalPages = 0;
        } else if (result > 0 && remainder == 0) {
            totalPages = result - 1;
        } else if (remainder != 0) {
            totalPages = result;
        }

        pageCounter.text = (currentPage + 1) + "/" + (totalPages + 1);
        for (int t = 0; t < totalPages; t++) {
            for (int i = 0; i < images.Length; i++) {
                if (images[i].name == "ToMainMenu") {
                    continue;
                }
                GameObject go = Instantiate(prefab) as GameObject;
                go.transform.SetParent(paren.transform);
                go.GetComponent<Image>().enabled = true;
                go.transform.position = new Vector3(images[i].transform.position.x, images[i].transform.position.y - (OFFSET * (t + 1)));
                go.transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
        images = GetComponentsInChildren<Image>();
        JSONObject progress = ConfigHandler.IsLoaded ? ConfigHandler.Config : ConfigHandler.LoadConfig();
        for (int i = 0; i < images.Length; i++) {
            if (images[i].name == "ToMainMenu") {
                continue;
            }
            if (i <= progress["Progress"].n) {
                Image image = images[i];
                Texture2D tex2d = new Texture2D(800, 600);
                try {
                    //image.sprite = Sprite.Create(levelDownloader.Thumbnails[i], new Rect(0, 0, tex2d.width, tex2d.height), image.sprite.pivot);
                    image.sprite = thumbnails[i];
                    Debug.Log(image.sprite.name);
                } catch (Exception) {
                    image.sprite = unknown.sprite;
                    image.color = unknown.color;
                }
                image.enabled = true;

                Text txt = image.GetComponentInChildren<Text>();
                txt.text = "Level " + (i + 1);
                txt.enabled = false;

                SelectableLevel id = image.gameObject.AddComponent<SelectableLevel>();
                id.SetScene(image.sprite.name.Replace("_Thumbnail", ""));
                id.SetCanClick(true);
            } else {
                Image image = images[i];
                image.sprite = unknown.sprite;
                image.color = unknown.color;
                image.enabled = true;

                Text txt = image.GetComponentInChildren<Text>();
                txt.text = "?";
                txt.enabled = false;

                SelectableLevel id = image.gameObject.AddComponent<SelectableLevel>();
                id.SetCanClick(false);
            }
        }
    }
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.DownArrow) && currentPage < totalPages && !move) {
            target = new Vector3(paren.position.x, paren.position.y + OFFSET);
            move = true;
            currentPage++;
            pageCounter.text = (currentPage + 1) + "/" + (totalPages + 1);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && currentPage > 0 && !move) {
            target = new Vector3(paren.position.x, paren.position.y - OFFSET);
            move = true;
            currentPage--;
            pageCounter.text = (currentPage + 1) + "/" + (totalPages + 1);
        }
        if (move) {
            paren.position = Vector3.MoveTowards(paren.position, target, 450 * Time.deltaTime);
            if (paren.position == target) {
                move = false;
            }
        }
    }
}

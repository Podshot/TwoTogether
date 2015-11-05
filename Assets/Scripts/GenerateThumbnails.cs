using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class GenerateThumbnails : MonoBehaviour {

    public Text pageCounter;
    public GameObject prefab;
    public Image unknown;
    public Transform paren;

    private int totalPages;
    private int currentPage = 0;
    private bool move;
    private Vector3 target;
    private const float OFFSET = 350f;

    void Start () {
        Image[] images = GetComponentsInChildren<Image>();
        // TODO: Change to the amount of levels in the "Levels" directory, do something for a number that isn't a multiple of 4
        int remainder;
        System.Math.DivRem(Directory.GetFiles(Application.dataPath + "/Levels/", "*.json").Length + 2, 4, out remainder);
        Debug.Log(remainder);
        Debug.Log(Directory.GetFiles(Application.dataPath + "/Levels/", "*.json").Length);
        totalPages = 2;
        pageCounter.text = (currentPage + 1) + "/" + (totalPages + 1);
        for (int t = 0; t < 2; t++) {
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
        JSONObject progress = new JSONObject(File.ReadAllText(Application.dataPath + "/data.json"));
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

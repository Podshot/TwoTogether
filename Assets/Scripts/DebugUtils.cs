using UnityEngine;
using System.Collections;

public class DebugUtils : MonoBehaviour {

    private int cooldown;

    void Start() {
        StartCoroutine(AssetBundleTest());
    }

    IEnumerator AssetBundleTest() {

        using (WWW www = new WWW("http://127.0.0.1:8000/levels.1_10")) {
            yield return www;
            AssetBundle assetBundle = www.assetBundle;
            Debug.LogError(assetBundle.Contains("Assets/Scenes/Level_1.unity"));
            
            Debug.LogError(assetBundle.GetAllScenePaths()[0]);
            assetBundle.LoadAllAssets();
            Application.LoadLevel(assetBundle.GetAllScenePaths()[0]);
            //Instantiate(gameObject);
            assetBundle.Unload(false);
        }

    }

    void Update() {
        if (cooldown == 0) {
            if (Input.GetKeyDown(KeyCode.BackQuote)) {
                ScreenCapture.CaptureScreenshot("test.png");
                cooldown = 10;
            }
        }
        if (cooldown != 0) {
            cooldown -= 1;
        }
	}
}

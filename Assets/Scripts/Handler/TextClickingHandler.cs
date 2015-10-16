using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextClickingHandler : MonoBehaviour {

    private Text textComponent;

    void Start() {
        textComponent = GetComponent<Text>();
    }
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            Vector2 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            //Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (textComponent.rectTransform.rect.Contains(mousePos)) {
                Debug.Log("Clicked");
            }
        }
	}
}

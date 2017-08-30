using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectableLevel : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField] private string sceneName;
    [SerializeField] private bool canClick;

    public void SetCanClick(bool b) {
        canClick = b;
    }

    public void SetScene(string name) {
        sceneName = name;
    }

    public void OnPointerDown(PointerEventData data) {
        if (canClick) {
            //GameObject ls = GameObject.Find("LevelSelector");
            //DontDestroyOnLoad(ls);
            //ls.GetComponent<LevelIdentity>().SetID(index);
            SceneManager.LoadScene(sceneName);
        }
    }

    public void OnPointerEnter(PointerEventData data) {
        GetComponentInChildren<Text>().enabled = true;
    }

    public void OnPointerExit(PointerEventData data) {
        GetComponentInChildren<Text>().enabled = false;
    }
}

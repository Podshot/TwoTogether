using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelIdentity : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {

    private bool set;
    private string id;
    private bool canClick;

    public void SetID(string i) {
        if (!set) {
            id = i;
            set = true;
        }
    }

    public string GetID() {
        return id;
    }

    public void SetCanClick(bool cc) {
        canClick = cc;
    }

    public void OnPointerDown(PointerEventData data) {
        if (canClick) {
            GameObject ls = GameObject.Find("LevelSelector");
            DontDestroyOnLoad(ls);
            ls.GetComponent<LevelIdentity>().SetID(id);
            Application.LoadLevel("TestScene");
        }
    }

    public void OnPointerEnter(PointerEventData data) {
        GetComponentInChildren<Text>().enabled = true;
    }

    public void OnPointerExit(PointerEventData data) {
        GetComponentInChildren<Text>().enabled = false;
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelIdentity : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {

    [SerializeField] private int index;
    private bool canClick;

    public void SetID(int num) {
        index = num;
    }

    public int GetIndex() {
        return index;
    }

    public void SetCanClick(bool cc) {
        canClick = cc;
    }

    public void OnPointerDown(PointerEventData data) {
        if (canClick) {
            GameObject ls = GameObject.Find("LevelSelector");
            DontDestroyOnLoad(ls);
            ls.GetComponent<LevelIdentity>().SetID(index);
            Application.LoadLevel("PrefabScene");
        }
    }

    public void OnPointerEnter(PointerEventData data) {
        GetComponentInChildren<Text>().enabled = true;
    }

    public void OnPointerExit(PointerEventData data) {
        GetComponentInChildren<Text>().enabled = false;
    }
}

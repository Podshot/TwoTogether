using UnityEngine;
using System.Collections;

interface IFadeable {

    IEnumerator FadeIn();

    IEnumerator FadeOut();

    void PartiallyFade(float alpha);
}

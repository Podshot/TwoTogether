using UnityEngine;
using System.Collections;

interface IFadeable {

    IEnumerator FadeIn();

    IEnumerator FadeOut();
}

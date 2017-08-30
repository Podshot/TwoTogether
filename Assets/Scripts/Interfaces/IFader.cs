using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwoTogether.Fading {

    public enum FadeType {
        FADE_IN,
        FADE_OUT,
        PARTIAL_FADE,
    }

    public abstract class IFader : MonoBehaviour {

        public void Awake() {
            GameManager.FadeActivated += FadeListener;
        }

        public void FadeListener(FadeType type, float value) {
            switch (type) {
                case FadeType.FADE_IN:
                    StartCoroutine(FadeIn());
                    break;
                case FadeType.FADE_OUT:
                    StartCoroutine(FadeOut());
                    break;
                case FadeType.PARTIAL_FADE:
                    PartialFade(value);
                    break;
                default:
                    break;
            }
        }

        public abstract IEnumerator FadeIn();

        public abstract IEnumerator FadeOut();

        public abstract void PartialFade(float value);
    }
}

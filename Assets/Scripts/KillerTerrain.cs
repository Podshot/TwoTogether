using UnityEngine;
using System.Collections;
using TwoTogether.Character;
using System;

namespace TwoTogether.SpecialTerrain {

    [RequireComponent(typeof(Collider2D))]
    public class KillerTerrain : MonoBehaviour, IFadeable {

        public CharacterType targetType;

        private SpawnHandler handler;
        private SpriteRenderer spriteRenderer;

        public void Start() {
            handler = GameObject.FindGameObjectWithTag("SpawnController").GetComponent<SpawnHandler>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.0f);
        }

        void OnCollisionEnter2D(Collision2D collision) {
            CharacterType type = collision.gameObject.GetComponent<Controller>().Type;
            if (type == targetType) {
                if (targetType == CharacterType.BigCube) {
                    handler.ResetBlueCube();
                } else if (targetType == CharacterType.SmallCube) {
                    handler.ResetRedCube();
                } else {
                    print(string.Format("This shouldn't ever display. TargetTag={0}", targetType.GetID()));
                }
            }
        }

        public CharacterType GetTargetTag() {
            return targetType;
        }

        public IEnumerator FadeOut() {
            for (float i = 1; i > 0f; i -= 0.025f) {
                if (spriteRenderer != null) {
                    Color color = spriteRenderer.color;
                    color.a = i;
                    spriteRenderer.color = color;
                }
                yield return null;
            }
            yield break;
        }

        public IEnumerator FadeIn() {
            for (float i = 0; i < 1f; i += 0.025f) {
                if (spriteRenderer != null) {
                    Color color = spriteRenderer.color;
                    color.a = i;
                    spriteRenderer.color = color;
                }
                yield return null;
            }
            yield break;
        }

        public void PartiallyFade(float alpha) {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
        }
    }
}
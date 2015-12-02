using UnityEngine;
using System.Collections;
using System;

namespace TwoTogether.Character {

    public enum CharacterType {
        SmallCube,
        BigCube,
        Unknown
    }

    static class CharacterTypeExtension {
        public static String GetID(this CharacterType c) {
            switch(c) {
                case CharacterType.SmallCube:
                    return "SmallCube";
                case CharacterType.BigCube:
                    return "BigCube";
                default:
                    return "";
            }
        }
        public static CharacterType GetEnumFromID(string id) {
            switch(id.ToLower()) {
                case "smallcube":
                    return CharacterType.SmallCube;
                case "bigcube":
                    return CharacterType.BigCube;
                default:
                    return CharacterType.Unknown;
            }
        }
    }

    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
    public class Controller : MonoBehaviour, IFadeable {

        // Editor Fields
        [SerializeField]
        private float speed;
        [SerializeField]
        private float jumpForce;
        [SerializeField]
        private float sensitivity;
        [SerializeField]
        private int jumpDelay;
        [SerializeField]
        private string axisID;


        // Components
        private Rigidbody2D rigidbody;
        private SpriteRenderer spriteRenderer;

        // State
        [SerializeField]
        private bool settingUp = false;
        private bool jumping = false;
        private int currentJumpDelay = 0;
        private ControlType currentControlType;

        // Misc
        private string horizontalAxis;
        private string verticalAxis;
        private CharacterType type;
        public CharacterType Type { get { return type; } }

        // Delegates that allow for easier modification of character movement
        delegate void MoveDelegate();
        MoveDelegate moveLeft;
        MoveDelegate moveRight;

        void Awake() {
            rigidbody = GetComponent<Rigidbody2D>();
            settingUp = true;
            moveLeft = MoveLeftNormal;
            moveRight = MoveRightNormal;
            currentControlType = ControlType.Normal;
            horizontalAxis = "Horizontal" + axisID;
            verticalAxis = "Vertical" + axisID;
            spriteRenderer = GetComponent<SpriteRenderer>();
            type = CharacterTypeExtension.GetEnumFromID(axisID);
        }

        public void Load() {
            if (spriteRenderer == null) {
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.0f);
            }
            settingUp = true;
        }

        void Update() {
            if (!settingUp) {
                if (Input.GetAxisRaw(horizontalAxis) < -sensitivity) {
                    moveLeft();
                }
                if (Input.GetAxisRaw(horizontalAxis) > sensitivity) {
                    moveRight();
                }
                if (Input.GetAxisRaw(verticalAxis) > sensitivity && !jumping) {
                    currentJumpDelay = jumpDelay;
                    rigidbody.AddForce(Vector2.up * jumpForce);
                    jumping = true;
                }

                if (Input.GetAxisRaw(horizontalAxis) == 0f) {
                    rigidbody.velocity = new Vector2(0f, rigidbody.velocity.y);
                }
            }
        }

        void FixedUpdate() {
            if (rigidbody.velocity.y == 0f && currentJumpDelay > 0) {
                currentJumpDelay -= 1;
            }
            if (rigidbody.velocity.y == 0f && currentJumpDelay == 0) {
                jumping = false;
            }
        }

        /* Start Movement Controllers
           Uses Rigidbody velocity based movement, these methods should be called from the delegates moveLeft/moveRight respectively
        */
        void MoveLeftNormal() {
            rigidbody.velocity = new Vector2(speed * Vector2.left.x, rigidbody.velocity.y);
        }

        void MoveRightNormal() {
            rigidbody.velocity = new Vector2(speed * Vector2.right.x, rigidbody.velocity.y);
        }
        // End Movement Controllers

        // Changes movement and collision detection
        public void SetControlType(ControlType type) {
            currentControlType = type;
            if (type == ControlType.Normal) {
                moveLeft = MoveLeftNormal;
                moveRight = MoveRightNormal;
            } else if (type == ControlType.Inverted) {
                moveLeft = MoveRightNormal;
                moveRight = MoveLeftNormal;
            }
        }

        // Public fuction for fading in the character
        public IEnumerator FadeIn() {
            for (float i = 0; i < 1f; i += 0.025f) {
                if (spriteRenderer != null) {
                    Color color = spriteRenderer.color;
                    color.a = i;
                    spriteRenderer.color = color;
                    yield return null;
                }
            }
            settingUp = false;
            yield break;
        }

        // Public fuction for fading out the character
        public IEnumerator FadeOut() {
            for (float i = 1; i > 0f; i -= 0.025f) {
                if (spriteRenderer != null) {
                    Color color = spriteRenderer.color;
                    color.a = i;
                    spriteRenderer.color = color;
                    yield return null;
                }
            }
            yield break;
        }

        // Returns the current ControlType
        public ControlType GetControlType() {
            return currentControlType;
        }

        public Rigidbody2D GetRigidbody() {
            return rigidbody;
        }
    }
}
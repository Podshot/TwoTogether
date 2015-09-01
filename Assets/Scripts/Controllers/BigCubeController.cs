using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class BigCubeController : MonoBehaviour {
	
	// Editor Fields
	[SerializeField] private float speed = 1.25f;
	[SerializeField] private float jumpForce = 200f;
	[SerializeField] private float sensitivity = 0.5f;
	[SerializeField] private int jumpDelay = 2;
	
	// Components
	private Rigidbody2D rigidbody;
	private SpriteRenderer renderer;
	
	// State
	private bool collidedOnRight = false;
	private bool collidedOnLeft = false;
	private bool fadeIn = false;
	private bool fadeOut = false;
	private bool settingUp = false;
	private bool jumping = false;
	private int currentJumpDelay = 0;
	private ControlType currentControlType;

    // Delegates that allow for easier modification of character movement
    delegate void MoveDelegate();
    MoveDelegate moveLeft;
    MoveDelegate moveRight;

    // Delegate to modify how collisions are detected for sides
    public delegate void CollideDelegate(string side, bool collided);
    public CollideDelegate SetCollided;

    void Awake() {
		rigidbody = GetComponent<Rigidbody2D>();
		settingUp = true;
        moveLeft = MoveLeftNormal;
        moveRight = MoveRightNormal;
        SetCollided = CollidedNormal;
		currentControlType = ControlType.Normal;
    }

	IEnumerator Start() {
		renderer = GetComponent<SpriteRenderer>();
		renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0.002f);
		yield return new WaitForSeconds(2);
		FadeIn();
		settingUp = false;
	}

	void Update() {
		if (fadeIn) {
			if (!(renderer.color.a > 1.0f)) {
				Color color = renderer.color;
				color.a *= 1.0625f;
				renderer.color = color;
			} else {
				fadeIn = false;
			}
		}
		if (fadeOut) {
			if (!(renderer.color.a < 0)) {
				Color color = renderer.color;
				color.a /= 1.0625f;
				renderer.color = color;
			} else {
				fadeOut = false;
			}
		}

		if (!(fadeOut || fadeIn || settingUp)) {
			if (Input.GetAxisRaw("HorizontalBig")<-sensitivity && !collidedOnLeft) {
                moveLeft();
			}
			if (Input.GetAxisRaw("HorizontalBig")>sensitivity && !collidedOnRight) {
                moveRight();
			}
			if (Input.GetAxisRaw("VerticalBig")>sensitivity && !jumping) {
                currentJumpDelay = jumpDelay;
                rigidbody.AddForce(Vector2.up * jumpForce);
				jumping = true;
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
       Uses Transform based movement, these methods should be called from the delegates moveLeft/moveRight respectively
    */
    void MoveLeftNormal() {
        transform.Translate(Vector3.left * Time.deltaTime * speed);
    }

    void MoveRightNormal() {
        transform.Translate(Vector3.right * Time.deltaTime * speed);
    }
    // End Movement Controllers

    // Changes movement and collision detection
    public void SetControlType(ControlType type) {
		currentControlType = type;
        if (type == ControlType.Normal) {
            moveLeft = MoveLeftNormal;
            moveRight = MoveRightNormal;
            SetCollided = CollidedNormal;
        } else if (type == ControlType.Inverted) {
            moveLeft = MoveRightNormal;
            moveRight = MoveLeftNormal;
            SetCollided = CollidedInverted;
        }
    }

    // Public fuction for fading in the character
    public void FadeIn() {
		fadeIn = true;
	}

    // Public fuction for fading out the character
    public void FadeOut() {
		fadeOut = true;
	}

	// Returns the current ControlType
	public ControlType GetControlType() {
		return currentControlType;
	}

	// Getter and Setter methods
	public void CollidedNormal(string side, bool collided) {
		if (side.Equals("RIGHT")) {
			collidedOnRight = collided;
		} else if (side.Equals("LEFT")) {
			collidedOnLeft = collided;
		}
	}

    void CollidedInverted(string side, bool collided) {
        if (side.Equals("LEFT")) {
            collidedOnRight = collided;
        } else if (side.Equals("RIGHT")) {
            collidedOnLeft = collided;
        }
    }
}

using System.Collections;
using UnityEngine;

public class BigCubeController : MonoBehaviour {

	private Rigidbody2D rigidbody;
	private float speed = 1.25f;
    private float jumpForce = 200f;
    private bool collidedOnRight = false;
	private bool collidedOnLeft = false;
	private bool fadeIn = false;
	private bool fadeOut = false;
	private SpriteRenderer renderer;
	private bool settingUp = false;
	private bool jumping = false;

    delegate void MoveDelegate();
    MoveDelegate moveLeft;
    MoveDelegate moveRight;

    public delegate void CollideDelegate(string side, bool collided);
    public CollideDelegate SetCollided;

    void Awake() {
		rigidbody = GetComponent<Rigidbody2D>();
		settingUp = true;
        moveLeft = MoveLeftNormal;
        moveRight = MoveRightNormal;
        SetCollided = CollidedNormal;
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
        if (rigidbody.IsSleeping() && !rigidbody.isKinematic) {
            rigidbody.isKinematic = true;
        } else if (!rigidbody.IsSleeping()) {
            rigidbody.isKinematic = false;
        }
		if (!(fadeOut || fadeOut || settingUp)) {
			if (Input.GetKey(KeyCode.A)) {
                //rigidbody.isKinematic = false;
                moveLeft();
			}
			if (Input.GetKey(KeyCode.D)) {
                //rigidbody.isKinematic = false;
                moveRight();
			}
			if (Input.GetKeyDown(KeyCode.W) && !jumping) {
                //rigidbody.isKinematic = false;
                rigidbody.AddForce(Vector2.up * jumpForce);
				jumping = true;
			}
            if (jumping) {
                //rigidbody.isKinematic = false;
            }
            if (Input.GetKeyUp(KeyCode.A)) {
                rigidbody.velocity = new Vector2(0f, rigidbody.velocity.y);
                //rigidbody.isKinematic = true;
            }
            if (Input.GetKeyUp(KeyCode.D)) {
                rigidbody.velocity = new Vector2(0f, rigidbody.velocity.y);
                //rigidbody.isKinematic = true;
            }
        }
        
	}

    // Start Movement Controllers
    void MoveLeftNormal() {
        rigidbody.velocity = new Vector2(speed * Vector2.left.x, rigidbody.velocity.y);
    }

    void MoveRightNormal() {
        rigidbody.velocity = new Vector2(speed * Vector2.right.x, rigidbody.velocity.y);
    }
    // End Movement Controllers

    public void SetControlType(ControlType type) {
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

    public void FadeIn() {
		fadeIn = true;
	}

	public void FadeOut() {
		fadeOut = true;
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (jumping) {
			jumping = false;
		}
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

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class KittyController : MonoBehaviour {
    private Rigidbody2D rigidbody2d;
    [SerializeField]
    private bool underPlayerControl = true;
    public Direction direction = Direction.Right;
    [SerializeField]
    private bool isWalking = false;
    [SerializeField]
    private float walkSpeed = 1.0f;

    public Animator animator;
    public string idleAnimName;
    public string walkAnimName;
    public AudioSource audioSource;

    void Start() {
        rigidbody2d = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update() {
        var input = Input.GetAxisRaw("Vertical");
        if (underPlayerControl && Mathf.Abs(input) > 0) {
            audioSource.Play();
        }
    }

    public void Move(float movement) {
        if (movement > 0) {
            ChangeDirection(Direction.Right);
            if (!isWalking) {
                animator.Play(walkAnimName);
                isWalking = true;
            }
            rigidbody2d.MovePosition(new Vector2(transform.position.x + (walkSpeed * Time.deltaTime), 0));
            // transform.Translate(walkSpeed * Time.deltaTime, 0, 0);
        }
        else if (movement < 0) {
            ChangeDirection(Direction.Left);
            if (!isWalking) {
                animator.Play(walkAnimName);
                isWalking = true;
            }
            rigidbody2d.MovePosition(new Vector2(transform.position.x - (walkSpeed * Time.deltaTime), 0));
            // transform.Translate(-walkSpeed * Time.deltaTime, 0, 0);
        }
        else {
            if (underPlayerControl && isWalking) {
                animator.Play(idleAnimName);
            }
            isWalking = false;
        }
    }

    public void ChangeDirection(Direction newDir) {
        if (direction != newDir) {
            if (direction == Direction.Left) {
                animator.transform.localScale = new Vector3(1, 1, 1);
                direction = Direction.Right;
            }
            else if (direction == Direction.Right) {
                animator.transform.localScale = new Vector3(-1, 1, 1);
                direction = Direction.Left;
            }
        }
    }

    public void AutomateKitty() {
        underPlayerControl = false;
        isWalking = false;
    }

    public void ReturnControlToPlayer() {
        animator.Play(idleAnimName);
        underPlayerControl = true;
    }

    public bool isUnderPlayerControl() {
        return underPlayerControl;
    }
}

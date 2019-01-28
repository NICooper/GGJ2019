using UnityEngine;
using System.Collections;

[RequireComponent(typeof(InteractionLinker))]
public class Interaction : MonoBehaviour {
    public bool interactionTriggered = false;
    public bool animationTriggered = false;
    private KittyController attachedPlayer = null;
    private Transform playerTransform = null;
    public ContextInputDirection directionButton = ContextInputDirection.Up;
    public Direction goalDirection = Direction.Right;
    public float interactionDuration = 4.0f;
    public Animator interactiveObjectAnimator;
    public string kittyAnimName;
    public string interactionAnimName;
    public string endAnimName;
    public Transform destinationPosition;
    public GameObject upArrowUI;
    public GameObject downArrowUI;
    public AudioSource interactionAudio;
    public float audioDelay = 0.0f;

    public Needs needFulfilled;
    public float fulfillmentAmount = 0.5f;

    void Update() {
        if (interactionTriggered && !animationTriggered && attachedPlayer != null) {
            Debug.Log("Interaction update running");
            if (transform.position.x != attachedPlayer.transform.position.x ||
                goalDirection != attachedPlayer.direction) {

                Debug.Log("Synchronizing");
                Synchronize();
            }
            else {
                Debug.Log("Starting interaction animation");
                animationTriggered = true;
                attachedPlayer.animator.Play(kittyAnimName);
                interactiveObjectAnimator.Play(interactionAnimName);
                interactionAudio.PlayDelayed(audioDelay);
                StartCoroutine(FinishInteraction());
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (!interactionTriggered) {
            if (directionButton == ContextInputDirection.Up) {
                upArrowUI.SetActive(true);
            }
            else if (directionButton == ContextInputDirection.Down) {
                downArrowUI.SetActive(true);
            }
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if (!interactionTriggered) {
            var input = Input.GetAxisRaw("Vertical");
            if ((input > 0 && directionButton == ContextInputDirection.Up) ||
                (input < 0 && directionButton == ContextInputDirection.Down)) {

                Debug.Log("Trigger activated");
                if (directionButton == ContextInputDirection.Up) {
                    upArrowUI.SetActive(false);
                }
                else if (directionButton == ContextInputDirection.Down) {
                    downArrowUI.SetActive(false);
                }
                interactionTriggered = true;
                attachedPlayer = other.GetComponent<KittyController>();
                playerTransform = attachedPlayer.transform;
                attachedPlayer.AutomateKitty();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (directionButton == ContextInputDirection.Up) {
            upArrowUI.SetActive(false);
        }
        else if (directionButton == ContextInputDirection.Down) {
            downArrowUI.SetActive(false);
        }
    }

    private void Synchronize() {
        var snapped = TrySnapPosition();
        Debug.Log(snapped);

        if (!snapped) {
            // flip toward goal
            float xDiff = transform.position.x - playerTransform.position.x;
            if (xDiff > 0 && attachedPlayer.direction == Direction.Left) {
                Debug.Log("Flipping");
                attachedPlayer.ChangeDirection(Direction.Right);
            }
            else if (xDiff < 0 && attachedPlayer.direction == Direction.Right) {
                Debug.Log("Flipping");
                attachedPlayer.ChangeDirection(Direction.Left);
            }
            else {
                // move to goal position
                Debug.Log("Moving to goal");
                attachedPlayer.Move(xDiff);
            }
        }
        else {
            // flip to goal direction
            if (attachedPlayer.direction != goalDirection) {
                Debug.Log("Aligning to goal");
                attachedPlayer.ChangeDirection(goalDirection);
            }
        }
    }

    private bool TrySnapPosition() {
        if (Mathf.Abs(transform.position.x - playerTransform.position.x) < 0.02f) {
            Debug.Log("Snapped");
            playerTransform.position = new Vector3(transform.position.x, playerTransform.position.y, playerTransform.position.z);
            return true;
        }
        return false;
    }

    IEnumerator FinishInteraction() {
        yield return new WaitForSeconds(interactionDuration);
        Debug.Log("Leaving animation");
        interactiveObjectAnimator.Play(endAnimName);
        if (destinationPosition != null) {
            // attachedPlayer.GetComponent<Rigidbody2D>().MovePosition(destinationPosition.position);
            playerTransform.position = destinationPosition.position;
        }
        attachedPlayer.ReturnControlToPlayer();
        var needSys = GameObject.FindGameObjectWithTag("GameManager").GetComponent<NeedSystem>();
        needSys.Fulfill(needFulfilled, fulfillmentAmount);
        animationTriggered = false;
        attachedPlayer = null;
        playerTransform = null;
        Debug.Log("Control returned to Player");
        gameObject.GetComponent<InteractionLinker>().StartRespawn();
    }
}

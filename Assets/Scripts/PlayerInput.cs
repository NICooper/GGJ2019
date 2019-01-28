using UnityEngine;

[RequireComponent(typeof(KittyController))]
public class PlayerInput : MonoBehaviour {
    private KittyController kc;

    void Start() {
        kc = gameObject.GetComponent<KittyController>();
    }    

    void Update() {
        if (kc.isUnderPlayerControl()) {
            var movement = Input.GetAxisRaw("Horizontal");
            if (movement > 0) {
                kc.Move(1);
            }
            else if (movement < 0) {
                kc.Move(-1);
            }
            else {
                kc.Move(0);
            }
        }
    }
}

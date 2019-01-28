using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Interaction))]
public class InteractionLinker : MonoBehaviour {
    public float respawnDelay = 20.0f;
    public int respawns = 10;
    public GameObject nextInteractionGameObject;
    public GameObject thisProp;
    private GameObject symbol;

    void Start() {
        symbol = gameObject.GetComponentInChildren<Animator>().gameObject;
    }

    public void StartRespawn() {
        Debug.Log("Disabling interaction");
        symbol.SetActive(false);
        StartCoroutine(EndRespawn());
    }

    IEnumerator EndRespawn() {
        if (respawns != 0) {
            yield return new WaitForSeconds(respawnDelay);

            Debug.Log("Reactivating interaction");
            symbol.SetActive(true);
            gameObject.GetComponent<Interaction>().interactionTriggered = false;
            respawns--;
        }
        else {
            if (nextInteractionGameObject != null) {
                thisProp.SetActive(false);
                nextInteractionGameObject.SetActive(true);
            }
        }
    }
}

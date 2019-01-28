using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedSystem : MonoBehaviour {
    private TimeSystem timeSystem;
    public List<float> needs = new List<float>();

    public float generalNeedLossSpeed = 0.01f;
    public float focussedNeedLossSpeed = 0.03f;
    private int focussedNeedIdx = -1;
    public float focussedChangeDelay = 8.0f;
    public float focussedChangeDelayVariance = 3.0f;
    public string gameoverFailureScene;

    void Start() {
        timeSystem = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TimeSystem>();

        var numNeeds = Enum.GetNames(typeof(Needs)).Length;
        for (int i = 0; i < numNeeds; i++) {
            needs.Add(1.0f);
        }
        ChangeFocussedNeed();
        StartCoroutine(FocussedNeedChangeDelay());
    }

    void Update() {
        if (!timeSystem.IsGameover()) {
            for (int i = 0; i < needs.Count; i++) {
                if (i == focussedNeedIdx) {
                    needs[i] -= focussedNeedLossSpeed * Time.deltaTime;
                }
                else {
                    needs[i] -= generalNeedLossSpeed * Time.deltaTime;
                }

                needs[i] = Mathf.Clamp01(needs[i]);
                if (needs[i] == 0.0f) {
                    StartCoroutine(timeSystem.EndGame(gameoverFailureScene));
                }
            }
        }
    }

    public void Fulfill(Needs need, float amount) {
        needs[(int)need] = Mathf.Clamp01(needs[(int)need] + amount);
    }

    private void ChangeFocussedNeed() {
        while (true) {
            var nextNeedIdx = UnityEngine.Random.Range(0, needs.Count - 1);
            if (nextNeedIdx != focussedNeedIdx) {
                focussedNeedIdx = nextNeedIdx;
                break;
            }
        }
    }

    IEnumerator FocussedNeedChangeDelay() {
        yield return new WaitForSeconds(UnityEngine.Random.Range(
                                        focussedChangeDelay - focussedChangeDelayVariance,
                                        focussedChangeDelay + focussedChangeDelayVariance));

        ChangeFocussedNeed();
        StartCoroutine(FocussedNeedChangeDelay());
    }
}

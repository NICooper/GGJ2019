using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeSystem : MonoBehaviour {
    public float startTime = 8.0f;
    public float endTime = 20.0f;
    public float timeSpeed = 0.1f;
    public float time;
    private bool isGameover = false;
    public GameObject disableUI;

    public string gameoverSuccessScene;

    void Start() {
        time = startTime;
    }

    void FixedUpdate() {
        if (!isGameover) {
            time += timeSpeed * Time.fixedDeltaTime;

            if (time >= endTime) {
                isGameover = true;
                // gameover success
                StartCoroutine(WinGame());
            }
        }
    }

    public bool IsGameover() {
        return isGameover;
    }

    public IEnumerator EndGame(string sceneName) {
        isGameover = true;
        gameObject.GetComponent<FadeOut>().BeginFade();
        yield return new WaitForSeconds(3.0f);

        SceneManager.LoadScene(sceneName);
    }

    public IEnumerator WinGame() {
        isGameover = true;
        gameObject.GetComponent<FadeOut>().BeginFade();
        yield return new WaitForSeconds(3.0f);

        Camera.main.GetComponent<Follow>().enabled = false;
        Camera.main.transform.position = new Vector3(0, -20, -10);
        disableUI.SetActive(false);
        gameObject.GetComponent<FadeIn>().Reset();
    }
}

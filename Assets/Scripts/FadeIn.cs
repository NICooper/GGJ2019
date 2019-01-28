using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour {
    public Image image;
    public float fadeDuration = 3.0f;
    private float fadeTime;
    private bool isFading = false;

    void Start() {
        Reset();
    }

    void Update() {
        if (isFading) {
            fadeTime -= Time.deltaTime;

            fadeTime = Mathf.Clamp(fadeTime, 0, fadeDuration);
            image.color = new Color(image.color.r, image.color.g, image.color.b, fadeTime / fadeDuration);

            if (fadeTime <= 0.0f) {
                this.enabled = false;
            }
        }
    }

    IEnumerator BeginFade() {
        yield return new WaitForSeconds(1f);
        isFading = true;
    }

    public void Reset() {
        this.enabled = true;
        fadeTime = fadeDuration;
        StartCoroutine(BeginFade());
    }
}

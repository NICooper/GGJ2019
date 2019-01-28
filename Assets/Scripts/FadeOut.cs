using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour {
    public Image image;
    public float fadeDuration = 3.0f;
    private float fadeTime;
    private bool isFading = false;

    void Start() {
        fadeTime = 0.0f;
    }

    void Update() {
        if (isFading) {
            fadeTime += Time.deltaTime;

            fadeTime = Mathf.Clamp(fadeTime, 0, fadeDuration);
            image.color = new Color(image.color.r, image.color.g, image.color.b, fadeTime / fadeDuration);

            if (fadeTime >= fadeDuration) {
                this.enabled = false;
            }
        }
    }

    public void BeginFade() {
        isFading = true;
    }
}

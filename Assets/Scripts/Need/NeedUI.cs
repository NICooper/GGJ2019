using UnityEngine;
using UnityEngine.UI;

public class NeedUI : MonoBehaviour {
    private NeedSystem needSystem;
    public Needs need;
    public Slider slider;
    public Image sliderFill;
    public Color goodColor;
    public float colorThreshold = 0.25f;
    public Color badColor;

    void Start() {
        needSystem = GameObject.FindGameObjectWithTag("GameManager").GetComponent<NeedSystem>();
    }

    void Update() {
        float value = needSystem.needs[(int)need];

        if (slider.value > colorThreshold && value <= colorThreshold) {
            sliderFill.color = badColor;
        }
        else if (slider.value <= colorThreshold && value > colorThreshold) {
            sliderFill.color = goodColor;
        }
        slider.value = value;
    }
}
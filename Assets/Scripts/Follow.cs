using UnityEngine;

public class Follow : MonoBehaviour {
    public Transform target;
    public float yMin = 3.0f;
    public float yMax = 10.0f;
    public float xMin = -10.0f;
    public float xMax = 10.0f;

    void Update() {
        transform.position = new Vector3(Mathf.Clamp(target.position.x, xMin, xMax), 
                                         Mathf.Clamp(target.position.y, yMin, yMax),
                                         -10.0f);
    }
}
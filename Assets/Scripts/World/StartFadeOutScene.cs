using UnityEngine;
using UnityEngine.UI;

public class StartFadeOutScene : MonoBehaviour {
    public Image BlackScreen;
    public float delay;
    private float timeStarted;

    private void Awake() {
        BlackScreen.color = new Color(0, 0, 0, 1);
    }

    private void Start() {
        timeStarted = Time.time;
    }

    private void Update() {
        if (Time.time - timeStarted > delay && BlackScreen.color.a > 0)
        {
            var step = Time.deltaTime * 0.35f;
            var newAlpha = Mathf.MoveTowards(BlackScreen.color.a, 0, step);
            BlackScreen.color = new Color(0, 0, 0, newAlpha);
        }
    }
}
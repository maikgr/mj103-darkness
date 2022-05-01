using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainFadeOut : MonoBehaviour
{
    [SerializeField]
    private Image screen;
    [SerializeField]
    private float delay;
    [SerializeField]
    private float duration;
    [SerializeField]
    private string startSceneName;
    private AnimationCurve fadeInCurve;
    private float animationDelta = -1f;
    public static MainFadeOut Instance;

    private void Start() {
        var instances = GameObject.FindGameObjectsWithTag("MainFadeOut");
        if (instances.Length > 1)
        {
            Destroy(this.gameObject);
        }
        Instance = this;
        screen.color = new Color(0, 0, 0, 0);
        fadeInCurve = new AnimationCurve(
            new Keyframe(0f, 0),
            new Keyframe(delay, 0),
            new Keyframe(duration - delay, 1f)
        );
    }

    public void FadeOutScreen() {
        animationDelta = 0f;
    }

    private void Update() {
        if(animationDelta > -1f)
        {
            animationDelta += Time.deltaTime;
            screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, fadeInCurve.Evaluate(animationDelta));

            if (animationDelta > fadeInCurve[fadeInCurve.length - 1].time)
            {
                animationDelta = -1f;
                SceneManager.LoadScene(startSceneName);
            }
        }
    }


}

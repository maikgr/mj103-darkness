using UnityEngine;
using UnityEngine.UI;
using System;

public class ScreenFadeController : MonoBehaviour
{
    [SerializeField]
    private Image screen;
    private AnimationCurve animationCurve;
    private float animationDelta = -1f;
    public static ScreenFadeController Instance;
    public Action onFadeEnds;

    private void Awake() {
        var instances = GameObject.FindObjectsOfType<ScreenFadeController>();
        if (instances.Length > 1)
        {
            Destroy(this.gameObject);
        }
        Instance = this;
    }

    public void FadeInScreen(float duration, float delay, Color screenColor, Action onFadeEnds = null) {
        screen.color = screenColor;
        animationCurve = new AnimationCurve(
            new Keyframe(0f, 0f),
            new Keyframe(delay, 0f),
            new Keyframe(duration - delay, 1f)
        );
        animationDelta = 0f;
        this.onFadeEnds = onFadeEnds;
    }

    public void FadeOutScreen(float duration, float delay, Color screenColor, Action onFadeEnds = null) {
        screen.color = screenColor;
        animationCurve = new AnimationCurve(
            new Keyframe(0f, 1f),
            new Keyframe(delay, 1f),
            new Keyframe(duration - delay, 0f)
        );
        animationDelta = 0f;
        this.onFadeEnds = onFadeEnds;
    }

    private void Update() {
        if(animationDelta > -1f)
        {
            animationDelta += Time.deltaTime;
            screen.color = new Color(screen.color.r, screen.color.g, screen.color.b, animationCurve.Evaluate(animationDelta));

            if (animationDelta > animationCurve[animationCurve.length - 1].time)
            {
                animationDelta = -1f;
                if (onFadeEnds != null)
                {
                    onFadeEnds.Invoke();
                }
            }
        }
    }

}

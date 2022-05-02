using UnityEngine;
using UnityEngine.UI;
using System;

public class ScreenFadeController : MonoBehaviour
{
    [SerializeField]
    private Image screen;
    private AnimationCurve[] colorCurves = new AnimationCurve[4]; // r, g, b, a
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

    private void SetAnimationCurve(float duration, float delay, Color startColor, Color targetColor)
    {
        colorCurves[0] = new AnimationCurve(
            new Keyframe(0f, startColor.r),
            new Keyframe(delay, startColor.r),
            new Keyframe(duration - delay, targetColor.r)
        );
        colorCurves[1] = new AnimationCurve(
            new Keyframe(0f, startColor.g),
            new Keyframe(delay, startColor.g),
            new Keyframe(duration - delay, targetColor.g)
        );
        colorCurves[2] = new AnimationCurve(
            new Keyframe(0f, startColor.b),
            new Keyframe(delay, startColor.b),
            new Keyframe(duration - delay, targetColor.b)
        );
        colorCurves[3] = new AnimationCurve(
            new Keyframe(0f, startColor.a),
            new Keyframe(delay, startColor.a),
            new Keyframe(duration - delay, targetColor.a)
        );
    }

    public void FadeInScreen(float duration, float delay, Color screenColor, Action onFadeEnds = null) {
        screen.color = screenColor;
        SetAnimationCurve(duration, delay, screenColor, new Color(screenColor.r, screenColor.g, screenColor.b, 1f));
        animationDelta = 0f;
        this.onFadeEnds = onFadeEnds;
    }

    public void FadeOutScreen(float duration, float delay, Color screenColor, Action onFadeEnds = null) {
        screen.color = screenColor;
        SetAnimationCurve(duration, delay, screenColor, new Color(screenColor.r, screenColor.g, screenColor.b, 0f));
        animationDelta = 0f;
        this.onFadeEnds = onFadeEnds;
    }

    public void FadeColorScreen(float duration, float delay, Color startColor, Color targetColor, Action onFadeEnds = null) {
        screen.color = startColor;
        SetAnimationCurve(duration, delay, startColor, targetColor);
        animationDelta = 0f;
        this.onFadeEnds = onFadeEnds;
    }

    private void Update() {
        if(animationDelta > -1f)
        {
            animationDelta += Time.deltaTime;
            screen.color = new Color(
                colorCurves[0].Evaluate(animationDelta),
                colorCurves[1].Evaluate(animationDelta),
                colorCurves[2].Evaluate(animationDelta),
                colorCurves[3].Evaluate(animationDelta)
            );

            if (animationDelta > colorCurves[0][colorCurves[0].length - 1].time)
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

using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerLightFlicker : MonoBehaviour
{
    [SerializeField]
    private Light2D stableLight;
    [SerializeField]
    private Light2D flickerLight;
    private AnimationCurve OuterRadiusAnimationCurve;
    private AnimationCurve InnerRadiusAnimationCurve;
    private AnimationCurve rColorAnimationCurve;
    private AnimationCurve gColorAnimationCurve;
    private AnimationCurve bColorAnimationCurve;
    private float lastStableLightRadius;
    private PlayerAnimationName animationName;
    private float animationDeltaTime;

    private void Start()
    {
        lastStableLightRadius = stableLight.pointLightOuterRadius;
        animationName = PlayerAnimationName.Idle;
        AdjustIdleLightFlicker(lastStableLightRadius);
    }

    public void PlayHurtAnimation()
    {
        AdjustHurtLight(lastStableLightRadius);
        animationDeltaTime = 0f;
        animationName = PlayerAnimationName.Hurt;
    }

    private void Update() {
        if (animationName == PlayerAnimationName.Idle)
        {
            animationDeltaTime += Time.deltaTime;
            flickerLight.pointLightOuterRadius = OuterRadiusAnimationCurve.Evaluate(animationDeltaTime);
            flickerLight.pointLightInnerRadius = InnerRadiusAnimationCurve.Evaluate(animationDeltaTime);
        }
        else if (animationName == PlayerAnimationName.Hurt)
        {
            animationDeltaTime += Time.deltaTime;
            flickerLight.pointLightOuterRadius = OuterRadiusAnimationCurve.Evaluate(animationDeltaTime);
            flickerLight.pointLightInnerRadius = InnerRadiusAnimationCurve.Evaluate(animationDeltaTime);
            flickerLight.color = new Color(
                rColorAnimationCurve.Evaluate(animationDeltaTime),
                gColorAnimationCurve.Evaluate(animationDeltaTime),
                bColorAnimationCurve.Evaluate(animationDeltaTime)
            );

            if (animationDeltaTime > rColorAnimationCurve[rColorAnimationCurve.length - 1].time)
            {
                animationName = PlayerAnimationName.Idle;
                // force stable check to happen
                lastStableLightRadius -= 0.1f;
            }
        }
    }

    private void FixedUpdate() {
        // if stable light adjusted, check every 'adjustment delay' seconds
        if (stableLight.pointLightOuterRadius != lastStableLightRadius)
        {
            lastStableLightRadius = stableLight.pointLightOuterRadius;
            AdjustIdleLightFlicker(lastStableLightRadius);
        }
    }

    private void AdjustIdleLightFlicker(float lightRadius)
    {
        float outerRadiusOffset = 0.15f;
        float innerRadisOffset = 0.3f;
        int frameCount = 10;

        OuterRadiusAnimationCurve = new AnimationCurve(
            Enumerable.Range(0, frameCount)
                .Select(index =>
                    new Keyframe
                    (
                        index * 0.2f,
                        lightRadius + Random.Range(0f, outerRadiusOffset)
                    ))
                .ToArray()
        );
        OuterRadiusAnimationCurve.postWrapMode = WrapMode.Loop;
        OuterRadiusAnimationCurve.preWrapMode = WrapMode.Loop;

        InnerRadiusAnimationCurve  = new AnimationCurve(
            Enumerable.Range(0, (int)frameCount)
                .Select(index =>
                    new Keyframe
                    (
                        index * 0.2f,
                        0 + Random.Range(0f, innerRadisOffset)
                    ))
                .ToArray()
        );
        InnerRadiusAnimationCurve.postWrapMode = WrapMode.Loop;
        InnerRadiusAnimationCurve.preWrapMode = WrapMode.Loop;
    }

    private void AdjustHurtLight(float lightRadius)
    {
        OuterRadiusAnimationCurve = new AnimationCurve(
            new Keyframe(0, lightRadius),
            new Keyframe(0.1f, lightRadius * 0.5f),
            new Keyframe(0.4f, lightRadius)
        );
        OuterRadiusAnimationCurve.postWrapMode = WrapMode.Once;
        OuterRadiusAnimationCurve.preWrapMode = WrapMode.Once;
        InnerRadiusAnimationCurve = new AnimationCurve(
            new Keyframe(0f, 0.1f)
        );
        InnerRadiusAnimationCurve.postWrapMode = WrapMode.Once;
        InnerRadiusAnimationCurve.preWrapMode = WrapMode.Once;

        ColorUtility.TryParseHtmlString("#EA5252", out Color hurtColor);
        rColorAnimationCurve = new AnimationCurve(
            new Keyframe(0, Color.white.r),
            new Keyframe(0.1f, hurtColor.r),
            new Keyframe(0.4f, Color.white.r)
        );
        gColorAnimationCurve = new AnimationCurve(
            new Keyframe(0, Color.white.g),
            new Keyframe(0.1f, hurtColor.g),
            new Keyframe(0.4f, Color.white.g)
        );
        bColorAnimationCurve = new AnimationCurve(
            new Keyframe(0, Color.white.b),
            new Keyframe(0.1f, hurtColor.b),
            new Keyframe(0.4f, Color.white.b)
        );
    }
}

public enum PlayerAnimationName
{
    Idle = 0,
    Hurt = 1
}

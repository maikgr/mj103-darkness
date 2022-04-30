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
    [SerializeField]
    private float adjustmentDelay;
    private AnimationCurve OuterRadiusAnimationCurve;
    private AnimationCurve InnerRadiusAnimationCurve;
    private float lastCheckTime;
    private float lastStableLightRadius;
    private float animationDeltaTime;

    private void Start()
    {
        animationDeltaTime = Time.deltaTime;
        lastCheckTime = Time.time;
        lastStableLightRadius = stableLight.pointLightOuterRadius;
        AdjustLightFlicker(lastStableLightRadius);
    }

    private void Update() {
        animationDeltaTime += Time.deltaTime;
        flickerLight.pointLightOuterRadius = OuterRadiusAnimationCurve.Evaluate(animationDeltaTime);
        flickerLight.pointLightInnerRadius = InnerRadiusAnimationCurve.Evaluate(animationDeltaTime);
    }

    private void FixedUpdate() {
        // if stable light adjusted, check every 'adjustment delay' seconds
        if (stableLight.pointLightOuterRadius != lastStableLightRadius && Time.time - lastCheckTime > adjustmentDelay)
        {
            lastCheckTime = Time.time;
            lastStableLightRadius = stableLight.pointLightOuterRadius;
            AdjustLightFlicker(lastStableLightRadius);
        }
    }

    private void AdjustLightFlicker(float lightRadius)
    {
        float outerRadiusOffset = 0.15f;
        float innerRadisOffset = 0.1f;
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
}
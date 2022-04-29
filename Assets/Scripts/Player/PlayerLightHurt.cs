using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System.Collections;

public class PlayerLightHurt : MonoBehaviour
{
    public Light2D stableLight;
    public Light2D flickeringLight;
    public float hurtOffset;
    public Color HurtColor;
    private float baseOuterRadius;
    private PlayerLightLevelController lightLevelController;

    private void Awake() {
        lightLevelController = GetComponent<PlayerLightLevelController>();
    }

    public void HurtFlicker() {
        lightLevelController.isLightRestricted = true;
        flickeringLight.color = HurtColor;
        baseOuterRadius = stableLight.pointLightOuterRadius;
        stableLight.pointLightOuterRadius = Mathf.Max(0, stableLight.pointLightOuterRadius - hurtOffset);
        StartCoroutine(LightColorChangeCoroutine());
    }

    private IEnumerator LightColorChangeCoroutine()
    {
        var interpolationTime = 0f;
        while (stableLight.pointLightOuterRadius < baseOuterRadius && 
            flickeringLight.color.g < Color.white.g && flickeringLight.color.b < Color.white.b)
        {
            flickeringLight.color = Color.Lerp(flickeringLight.color, Color.white, interpolationTime);
            stableLight.pointLightOuterRadius = Mathf.Lerp(stableLight.pointLightOuterRadius, baseOuterRadius, interpolationTime);
            interpolationTime += Time.fixedDeltaTime * 0.5f;
            yield return new WaitForFixedUpdate();
        }
        flickeringLight.color = Color.white;
        stableLight.pointLightOuterRadius = baseOuterRadius;
        lightLevelController.isLightRestricted = false;
    }
}
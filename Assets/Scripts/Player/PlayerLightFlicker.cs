using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System.Collections;

public class PlayerLightFlicker : MonoBehaviour
{
    public Light2D stableLight;
    public Light2D flickeringLight;
    public float flickerOffset;

    private void Start()
    {
        StartCoroutine(LightFlickerCoroutine());
    }

    private IEnumerator LightFlickerCoroutine()
    {
        // Light flickers
        while (true)
        {
            flickeringLight.pointLightOuterRadius = stableLight.pointLightOuterRadius - Random.Range(0f, flickerOffset);
            flickeringLight.pointLightInnerRadius = stableLight.pointLightInnerRadius - Random.Range(0f, flickerOffset * 3);
            yield return new WaitForSeconds(Random.Range(0, 0.2f));
        }
    }
}
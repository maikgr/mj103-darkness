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
            flickeringLight.pointLightOuterRadius = Mathf.Max(0,stableLight.pointLightOuterRadius - Random.Range(0f, flickerOffset));
            flickeringLight.pointLightInnerRadius = Mathf.Max(0, stableLight.pointLightInnerRadius - Random.Range(0f, flickerOffset * 3));
            yield return new WaitForSeconds(Random.Range(0, 0.2f));
        }
    }
}
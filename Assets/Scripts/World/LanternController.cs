using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System.Collections;

public class LanternController : MonoBehaviour {
    [SerializeField]
    private int useAmount;
    [SerializeField]
    private int recoverAmount;
    [SerializeField]
    private float activeLanternLightRadius;
    [SerializeField]
    private float inactiveLanternLightRadius;
    private Light2D lanternLight;
    private void Awake() {
        lanternLight = GetComponentInChildren<Light2D>();
        lanternLight.pointLightOuterRadius = activeLanternLightRadius;
    }

    public int UseLantern()
    {
        if (useAmount == 0) {
            return 0;
        }
        --useAmount;
        if (useAmount == 0) {
            StartCoroutine(DeactivateLantern());
        }
        return recoverAmount;
    }

    private IEnumerator DeactivateLantern()
    {
        var t = 0f;
        while(lanternLight.pointLightOuterRadius > inactiveLanternLightRadius)
        {
            lanternLight.pointLightOuterRadius = Mathf.Lerp(lanternLight.pointLightOuterRadius, inactiveLanternLightRadius, t);
            t += Time.deltaTime * 0.75f;
            yield return new WaitForFixedUpdate();
        }
    }
}
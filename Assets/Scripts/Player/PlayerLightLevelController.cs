using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerLightLevelController : MonoBehaviour {
    public Light2D stableLight;
    public float maxRadius;
    public float minRadius;
    public bool isLightRestricted;
    private float expansionTime = 0f;
    private float shrinkTime = 0f;
    private bool isExpanding;
    private bool isShrinking;

    private void Start() {
        stableLight.pointLightOuterRadius = minRadius;
    }

    private void FixedUpdate() {
        if (!isLightRestricted)
        {
            if (Input.GetButton("Fire1"))
            {
                isExpanding = true;
                isShrinking = false;
                stableLight.pointLightOuterRadius = Mathf.Lerp(stableLight.pointLightOuterRadius, maxRadius, expansionTime);
                expansionTime += Time.fixedDeltaTime * 0.15f;
            }
            else if (stableLight.pointLightOuterRadius > minRadius)
            {
                isShrinking = true;
                isExpanding = false;
                stableLight.pointLightOuterRadius = Mathf.Lerp(stableLight.pointLightOuterRadius, minRadius, shrinkTime);
                shrinkTime += Time.fixedDeltaTime * 0.15f;
            }
            if (!isExpanding && expansionTime > 0)
            {
                expansionTime = 0f;
            }
            if (!isShrinking && shrinkTime > 0)
            {
                shrinkTime = 0f;
            }
        }
    }
}
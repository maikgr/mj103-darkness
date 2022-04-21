using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ShadowHandsController : MonoBehaviour
{
    public Light2D light2dController;
    public ShadowHandPositionEnum Position;
    public ShadowHandSizeEnum Size;
    private Vector2 basePos;
    private float baseRadius;
    // Start is called before the first frame update
    void Start()
    {
        basePos = new Vector2(transform.localPosition.x, transform.localPosition.y);
        baseRadius = light2dController.pointLightOuterRadius;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.localPosition = new Vector2(
                light2dController.pointLightOuterRadius/baseRadius * basePos.x,
                light2dController.pointLightOuterRadius/baseRadius * basePos.y
            );
    }
}

public enum ShadowHandPositionEnum
{
    Position0 = 0,
    Position1 = 1,
    Position2 = 2,
    Position3 = 3,
    Position4 = 4,
    Position5 = 5,
    Position6 = 6,
    Position7 = 7,
}

public enum ShadowHandSizeEnum
{
    Small = 0,
    Medium = 1,
    Big = 2,
}

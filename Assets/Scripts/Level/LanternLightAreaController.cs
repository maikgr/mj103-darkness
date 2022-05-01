using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LanternLightAreaController : MonoBehaviour
{
    private CircleCollider2D lightCollider;
    private Light2D lightArea;
    
    private void Start() {
        lightCollider = GetComponent<CircleCollider2D>();
        lightArea = GetComponent<Light2D>();
    }

    private void Update() {
        lightCollider.radius = lightArea.pointLightOuterRadius;
    }
}

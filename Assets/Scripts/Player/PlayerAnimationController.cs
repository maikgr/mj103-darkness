using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator LightFlickerAnimator;
    [SerializeField]
    private AnimationCurve LightFlickerAnimationCurve;
    [SerializeField]
    private PlayerLightLevelController LightLevelController;

    private void Start() {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionScreenController : MonoBehaviour
{
    public float fadeDuration;
    private void Start() {
        ScreenFadeController.Instance.FadeOutScreen(
            fadeDuration,
            0f,
            Color.black
        );
    }
}

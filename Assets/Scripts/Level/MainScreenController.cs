using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScreenController : MonoBehaviour
{
    public float startFadeOutDuration;
    private void Start()
    {
        ScreenFadeController.Instance.FadeOutScreen(
            startFadeOutDuration,
            0f,
            Color.black
        );
    }
}

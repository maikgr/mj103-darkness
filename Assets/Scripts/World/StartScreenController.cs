using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreenController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ScreenFadeController.Instance.FadeOutScreen(
            5f,
            2f,
            Color.black
        );
    }
}

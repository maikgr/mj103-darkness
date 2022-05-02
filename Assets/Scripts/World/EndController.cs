using UnityEngine;

public class EndController : MonoBehaviour
{
    private void Start() {
        ScreenFadeController.Instance.FadeOutScreen(
            2f,
            0,
            Color.black
        );
    }
}

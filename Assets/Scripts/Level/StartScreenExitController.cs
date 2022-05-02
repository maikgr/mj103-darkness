using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenExitController : MonoBehaviour {
    public string gameSceneName;
    public AudioSource transitionSfx;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovementController>().ModifySpeed(0);
            transitionSfx.Play();
            ScreenFadeController.Instance.FadeInScreen(
                transitionSfx.clip.length,
                0,
                new Color(0, 0, 0, 0),
                () => SceneManager.LoadScene(gameSceneName)
            );
        }
    }
}

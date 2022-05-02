using UnityEngine;
using UnityEngine.SceneManagement;

public class MainExitController : MonoBehaviour {
    public string transitionSceneName;
    public string endSceneName;
    public AudioSource AudioSource;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            AudioSource.Play();
            ScreenFadeController.Instance.FadeInScreen(
                AudioSource.clip.length,
                0,
                new Color(0, 0, 0, 0),
                () =>
                {
                    if (GameController.Instance.currentLevel < 4)
                    {
                        SceneManager.LoadScene(transitionSceneName);
                    }
                    else
                    {
                        SceneManager.LoadScene(endSceneName);
                    }
                }
            );
        }
    }
}

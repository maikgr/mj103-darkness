using UnityEngine;
using UnityEngine.SceneManagement;


public class TransitionExitController : MonoBehaviour
{
    public string mainSceneName;
    public string endSceneName;
    public AudioSource audioSource;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            audioSource.Play();
            ScreenFadeController.Instance.FadeInScreen(
                audioSource.clip.length,
                0,
                new Color(0, 0, 0, 0),
                () => {
                    var nextLevel = GameController.Instance.currentLevel + 1;
                    if (nextLevel < 4)
                    {
                        GameController.Instance.currentLevel = nextLevel;
                        SceneManager.LoadScene(mainSceneName);
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

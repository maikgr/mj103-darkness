using UnityEngine;
using UnityEngine.SceneManagement;


public class TransitionExitController : MonoBehaviour
{
    public string mainSceneName;
    public string endSceneName;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
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
    }
}

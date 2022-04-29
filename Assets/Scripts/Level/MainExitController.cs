using UnityEngine;
using UnityEngine.SceneManagement;

public class MainExitController : MonoBehaviour {
    public string transitionSceneName;
    public string endSceneName;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
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
    }
}

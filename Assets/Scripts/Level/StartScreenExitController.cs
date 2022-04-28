using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenExitController : MonoBehaviour {
    public string sceneName;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}

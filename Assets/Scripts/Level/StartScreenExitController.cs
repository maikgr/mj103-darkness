using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenExitController : MonoBehaviour {
    public string scenePath;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag.Equals("Player"))
        {
            SceneManager.LoadScene(scenePath);
        }
    }
}

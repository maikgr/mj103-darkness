using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public int currentLevel;
    private void Awake() {
        var controllers  = GameObject.FindGameObjectsWithTag("GameController");
        if (controllers.Length > 1)
        {
            Destroy(this.gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}

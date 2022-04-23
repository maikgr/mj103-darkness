using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private LevelGenerator levelGenerator;

    private void Start()
    {
        levelGenerator = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelGenerator>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "ExitTile")
        {
            levelGenerator.RestartLevel();
        }
    }
}

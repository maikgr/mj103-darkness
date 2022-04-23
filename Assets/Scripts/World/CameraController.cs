using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mj103Scripts.Level;

public class CameraController : MonoBehaviour
{
    private Transform player;
    private Camera mainCamera;
    public LevelGenerator levelGenerator;
    private LevelInstance levelInstance;
    void Start() {
        mainCamera = GetComponent<Camera>();
    }

    void LateUpdate() {
        if (player != null && levelInstance != null)
        {
            MoveCamera(player.transform.position);
        }
        else if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        else if (levelInstance == null)
        {
            levelInstance = levelGenerator.levelInstance;
        }
    }

    private void MoveCamera(Vector2 playerPos) {
        // float xOffset = 0.5f;
        float xMin = 4.5f;
        float xMax = 28.5f;
        float yMin = -23f;
        float yMax = -2.5f;

        float xPos = Mathf.Clamp(playerPos.x, xMin, xMax);
        float yPos = Mathf.Clamp(playerPos.y, yMin, yMax);
        transform.position = new Vector3(xPos, yPos, transform.position.z);
    }
}

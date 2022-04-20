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

    public void MoveCamera(Vector2 playerPos) {
        float xOffset = 0.5f;
        float xMin = mainCamera.orthographicSize * 2 + xOffset;
        float xMax = levelInstance.XSize - (mainCamera.orthographicSize * 2 - xOffset);
        float yMin = -levelInstance.YSize + mainCamera.orthographicSize - 1;
        float yMax = -mainCamera.orthographicSize;

        float xPos = Mathf.Clamp(playerPos.x, xMin, xMax);
        float yPos = Mathf.Clamp(playerPos.y, yMin, yMax);
        transform.position = new Vector3(xPos, yPos, transform.position.z);
    }
}

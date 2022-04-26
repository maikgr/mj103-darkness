using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mj103Scripts.Level;

public class CameraController : MonoBehaviour
{
    private Transform player;
    private Camera mainCamera;
    public Vector2 minPoint;
    public Vector2 maxPoint;
    void Start() {
        mainCamera = GetComponent<Camera>();
    }

    void LateUpdate() {
        if (player != null)
        {
            MoveCamera(player.transform.position);
        }
        else if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void MoveCamera(Vector2 playerPos) {
        float xPos = Mathf.Clamp(playerPos.x, minPoint.x, maxPoint.x);
        float yPos = Mathf.Clamp(playerPos.y, minPoint.y, maxPoint.y);
        transform.position = new Vector3(xPos, yPos, transform.position.z);
    }
}

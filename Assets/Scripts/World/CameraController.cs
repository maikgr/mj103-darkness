using UnityEngine;

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
        transform.position = new Vector3(playerPos.x, playerPos.y, transform.position.z);
    }
}

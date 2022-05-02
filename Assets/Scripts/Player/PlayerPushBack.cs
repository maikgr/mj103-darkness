using UnityEngine;
using System.Collections;

public class PlayerPushBack : MonoBehaviour
{
    public float pushBackAmount;
    public float delaySeconds;
    private PlayerMovementController movementController;

    private void Awake() {
        movementController = GetComponent<PlayerMovementController>();
    }

    public void PushBack(Vector3 otherPos) {
        movementController.isMovementRestricted = true;
        StartCoroutine(PushPlayerBack(otherPos));
    }

    private IEnumerator PushPlayerBack(Vector3 otherPos)
    {
        var difference = otherPos - this.transform.position;
        this.transform.localPosition = new Vector2(
            transform.localPosition.x - difference.x * pushBackAmount,
            transform.localPosition.y - difference.y * pushBackAmount
        );
        yield return new WaitForSeconds(delaySeconds);
        movementController.isMovementRestricted = false;
    }
}
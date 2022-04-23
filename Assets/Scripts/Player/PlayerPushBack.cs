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

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Enemy")
        {
            movementController.isAnimating = true;
            StartCoroutine(PushPlayerBack(other.transform.position));
        }
    }

    private IEnumerator PushPlayerBack(Vector3 otherPos)
    {
        var difference = otherPos - this.transform.position;
        this.transform.localPosition = new Vector2(
            transform.localPosition.x - difference.x * pushBackAmount,
            transform.localPosition.y - difference.y * pushBackAmount
        );
        yield return new WaitForSeconds(delaySeconds);
        movementController.isAnimating = false;
    }
}
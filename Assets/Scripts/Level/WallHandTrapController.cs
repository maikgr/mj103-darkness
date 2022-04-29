using UnityEngine;

public class WallHandTrapController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerPushBack>().PushBack(this.transform.position);
            other.GetComponent<PlayerLightHurt>().HurtFlicker();
        }
    }
}

using UnityEngine;

public class WallHandTrapController : MonoBehaviour
{
    public float damage;
    [SerializeField]
    private AudioSource AudioSource;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            AudioSource.Play();
            var playerController = other.GetComponent<PlayerController>();
            playerController.SetHealth(playerController.CurrentHealth - damage);
            other.GetComponent<PlayerPushBack>().PushBack(this.transform.position);
            other.GetComponent<PlayerLightFlicker>().PlayHurtAnimation();
        }
    }
}

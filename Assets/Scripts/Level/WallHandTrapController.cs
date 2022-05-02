using UnityEngine;

public class WallHandTrapController : MonoBehaviour
{
    [SerializeField]
    private AudioSource AudioSource;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            AudioSource.Play();
            other.GetComponent<PlayerPushBack>().PushBack(this.transform.position);
            other.GetComponent<PlayerLightFlicker>().PlayHurtAnimation();
        }
    }
}

using UnityEngine;

public class PoolTrapController : MonoBehaviour
{
    public Animator poolAnimator;
    public float damageAmount;
    public float damageInterval;
    [Range(0, 1)]
    public float speedModifier;
    [SerializeField]
    private AudioSource AudioSource;
    private PlayerController playerController;
    private PlayerLightFlicker playerLightHurt;
    private PlayerMovementController playerMovementController;
    private float lastDamagedTime;

    private void FixedUpdate()
    {
        if (playerController != null
            && playerLightHurt != null
            && playerMovementController != null
            && Time.time - lastDamagedTime > damageInterval)
        {
            lastDamagedTime = Time.time;
            playerController.SetHealth(playerController.CurrentHealth - damageAmount);
            playerLightHurt.PlayHurtAnimation();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            AudioSource.Play();
            lastDamagedTime = Time.time;
            playerController = other.GetComponent<PlayerController>();
            playerLightHurt = other.GetComponent<PlayerLightFlicker>();
            playerMovementController = other.GetComponent<PlayerMovementController>();
            playerMovementController.ModifySpeed(speedModifier);
            poolAnimator.SetBool("IsTrapActive", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            AudioSource.Stop();
            playerMovementController.ResetSpeedModifier();
            playerController = null;
            playerLightHurt = null;
            playerMovementController = null;
            poolAnimator.SetBool("IsTrapActive", false);
        }
    }
}

using UnityEngine;

public class PoolTrapController : MonoBehaviour
{
    public Animator poolAnimator;
    public float damageAmount;
    public float damageInterval;
    public float speedModifier;
    private PlayerController playerController;
    private PlayerLightHurt playerLightHurt;
    private PlayerMovementController playerMovementController;
    private float lastDamagedTime;
    private float originalSpeed;

    private void FixedUpdate()
    {
        if (playerController != null
            && playerLightHurt != null
            && playerMovementController != null
            && Time.time - lastDamagedTime > damageInterval)
        {
            lastDamagedTime = Time.time;
            playerController.SetHealth(playerController.CurrentHealth - damageAmount);
            playerLightHurt.HurtFlicker();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            lastDamagedTime = Time.time;
            playerController = other.GetComponent<PlayerController>();
            playerLightHurt = other.GetComponent<PlayerLightHurt>();
            playerMovementController = other.GetComponent<PlayerMovementController>();
            originalSpeed = playerMovementController.speed + 0f;
            playerMovementController.speed = speedModifier;
            poolAnimator.SetBool("IsTrapActive", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            Debug.Log("original " + originalSpeed);
            playerMovementController.speed = originalSpeed;
            playerController = null;
            playerLightHurt = null;
            playerMovementController = null;
            poolAnimator.SetBool("IsTrapActive", false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField]
    private float speed;
    public bool isAnimating;
    private float speedModifier = 1f;

    // Update is called once per frame
    void FixedUpdate()
    {
        // Movement
        if (!isAnimating) {
            transform.localPosition = new Vector2(
                    transform.localPosition.x + Input.GetAxis("Horizontal") * Time.deltaTime * speed * speedModifier,
                    transform.localPosition.y + Input.GetAxis("Vertical") * Time.deltaTime * speed * speedModifier
                );
        }
    }

    public void ModifySpeed(float modifier)
    {
        this.speedModifier = modifier;
    }

    public void ResetSpeedModifier()
    {
        this.speedModifier = 1f;
    }
}

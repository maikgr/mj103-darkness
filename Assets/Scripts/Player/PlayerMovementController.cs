using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public float speed;
    public bool isAnimating;

    // Update is called once per frame
    void FixedUpdate()
    {
        // Movement
        if (!isAnimating) {
            transform.localPosition = new Vector2(
                    transform.localPosition.x + Input.GetAxis("Horizontal") * Time.deltaTime * speed,
                    transform.localPosition.y + Input.GetAxis("Vertical") * Time.deltaTime * speed
                );
        }
    }
}

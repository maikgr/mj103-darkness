using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float initialHealth;
    public float CurrentHealth { get; private set; }
    public List<SpriteRenderer> sprites;
    private PlayerLightLevelController lightLevelController;
    private int currentSpriteIndex;

    private float CurrentHealthPercentage
    {
        get
        {
            return CurrentHealth/initialHealth;
        }
    }

    private void Start()
    {
        lightLevelController = GetComponent<PlayerLightLevelController>();
        SetHealth(initialHealth);
    }

    private void FixedUpdate() {
        if (Input.GetButton("Fire1"))
        {
            SetHealth(CurrentHealth - Time.fixedDeltaTime * 20);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Enemy")
        {
            SetHealth(CurrentHealth - 50f);
        }
        if (other.tag == "Lantern")
        {
            SetHealth(CurrentHealth + other.GetComponent<LanternController>().UseLantern());
        }
    }

    private void SetHealth(float amount)
    {
        CurrentHealth = Mathf.Clamp(amount, 0, initialHealth);
        if (CurrentHealthPercentage > 0.7f)
        {
            lightLevelController.maxRadius = 4;
            SetSprite(3);
        }
        else if (CurrentHealthPercentage > 0.5f)
        {
            lightLevelController.maxRadius = 2;
            SetSprite(2);
        }
        else if (CurrentHealthPercentage > 0.25f)
        {
            lightLevelController.maxRadius = 1;
            SetSprite(1);
        }
        else if (CurrentHealthPercentage > 0.1f)
        {
            lightLevelController.maxRadius = 0.5f;
            SetSprite(0);
        }
    }

    private void SetSprite(int index)
    {
        if (currentSpriteIndex != index)
        {
            currentSpriteIndex = index;
            var current = GetComponentInChildren<SpriteRenderer>();
            if (current != null)
            {
                GameObject.Destroy(current.gameObject);
            }
            var sprite = Instantiate(sprites[index], this.transform.position, Quaternion.identity);
            sprite.transform.SetParent(this.transform); 
            sprite.transform.localPosition = Vector3.zero;
            sprite.transform.SetSiblingIndex(0);
        }
    }
}

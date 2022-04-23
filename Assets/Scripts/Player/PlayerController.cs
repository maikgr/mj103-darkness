using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float initialHealth;
    public float CurrentHealth { get; private set; }
    public SpriteRenderer spriteRenderer;
    public List<Sprite> sprites;
    private LevelGenerator levelGenerator;
    private PlayerLightLevelController lightLevelController;

    private float CurrentHealthPercentage
    {
        get
        {
            return CurrentHealth/initialHealth;
        }
    }

    private void Start()
    {
        levelGenerator = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelGenerator>();
        lightLevelController = GetComponent<PlayerLightLevelController>();
        SetHealth(initialHealth);
        UIManager.Instance.SetHealth(CurrentHealthPercentage * 100);
    }

    private void FixedUpdate() {
        if (Input.GetButton("Fire1"))
        {
            SetHealth(CurrentHealth - Time.fixedDeltaTime * 20);
            UIManager.Instance.SetHealth(CurrentHealthPercentage * 100);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "ExitTile")
        {
            levelGenerator.RestartLevel();
        }
        if (other.tag == "Enemy")
        {
            SetHealth(CurrentHealth - 50f);
            UIManager.Instance.SetHealth(CurrentHealthPercentage * 100);
        }
    }

    private void SetHealth(float amount)
    {
        CurrentHealth = Mathf.Max(0, amount);
        if (CurrentHealthPercentage > 0.7f)
        {
            spriteRenderer.sprite = sprites[3];
            lightLevelController.maxRadius = 4;
        }
        else if (CurrentHealthPercentage > 0.5f)
        {
            spriteRenderer.sprite = sprites[2];
            lightLevelController.maxRadius = 2;
        }
        else if (CurrentHealthPercentage > 0.25f)
        {
            spriteRenderer.sprite = sprites[1];
            lightLevelController.maxRadius = 1;
        }
        else if (CurrentHealthPercentage > 0.1f)
        {
            spriteRenderer.sprite = sprites[0];
            lightLevelController.maxRadius = 0.5f;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float initialHealth;
    public float CurrentHealth { get; private set; }
    [SerializeField]
    private AudioSource AudioSource;
    public List<SpriteRenderer> sprites;
    public PlayerLightLevelController lightLevelController;
    public PlayerMovementController movementController;
    private int currentSpriteIndex;
    private LanternController nearbyLantern;
    private EndLanternController endLantern;
    public string startSceneName;

    private float CurrentHealthPercentage
    {
        get
        {
            return CurrentHealth/initialHealth;
        }
    }

    private void Start()
    {
        SetHealth(initialHealth);
    }

    private void Update() {
        if (Input.GetButton("Fire1") && CurrentHealth > 0)
        {
            SetHealth(CurrentHealth - Time.fixedDeltaTime * 15f);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (nearbyLantern != null)
            {
                SetHealth(CurrentHealth + nearbyLantern.UseLantern());
            }
            if (endLantern != null)
            {
                endLantern.UseLantern();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Lantern")
        {
            nearbyLantern = other.GetComponent<LanternController>();
        }
        if (other.tag == "EndLantern")
        {
            endLantern = other.GetComponent<EndLanternController>();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Lantern")
        {
            nearbyLantern = null;
        }
        if (other.tag == "EndLantern")
        {
            endLantern = null;
        }
    }

    public void SetHealth(float amount)
    {
        

        CurrentHealth = Mathf.Clamp(amount, 0, initialHealth);
        if (CurrentHealthPercentage > 0.5f)
        {
            if (lightLevelController != null) lightLevelController.maxRadius = 4;
            SetSprite(3);
        }
        else if (CurrentHealthPercentage > 0.3f)
        {
            if (lightLevelController != null) lightLevelController.maxRadius = 2;
            SetSprite(2);
        }
        else if (CurrentHealthPercentage > 0.15f)
        {
            if (lightLevelController != null) lightLevelController.maxRadius = 1;
            SetSprite(1);
        }
        else if (CurrentHealthPercentage > 0.05f)
        {
            if (lightLevelController != null) lightLevelController.maxRadius = 0.5f;
            SetSprite(0);
        }
        else if (CurrentHealthPercentage <= 0f)
        {
            lightLevelController.isLightRestricted = true;
            movementController.ModifySpeed(0);
            AudioSource.Play();
            ScreenFadeController.Instance.FadeInScreen(
                AudioSource.clip.length,
                0f,
                new Color(0, 0, 0, 0),
                () => SceneManager.LoadScene(startSceneName)
            );
        }
    }

    private void SetSprite(int index)
    {
        if (currentSpriteIndex != index)
        {
            var spriteName = "PlayerSprite";
            currentSpriteIndex = index;
            var current = this.transform.Find(spriteName);
            if (current != null)
            {
                GameObject.Destroy(current.gameObject);
            }
            var sprite = Instantiate(sprites[index], this.transform.position, Quaternion.identity, this.transform);
            sprite.name = spriteName;
            sprite.transform.localPosition = Vector3.zero;
        }
    }
}

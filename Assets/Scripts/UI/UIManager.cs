using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text HealthText;
    private PlayerController playerController;

    private void LateUpdate() {
        if (playerController == null)
        {
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
        else
        {
            SetHealth(playerController.CurrentHealth);
        }
    }

    public void SetHealth(float health)
    {
        HealthText.text = health.ToString("F2");
    }
}

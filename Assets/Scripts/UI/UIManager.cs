using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public Text HealthText;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void SetHealth(float health)
    {
        HealthText.text = health.ToString("F2");
    }
}

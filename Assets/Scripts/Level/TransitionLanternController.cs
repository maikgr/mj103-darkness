using UnityEngine;
using TMPro;
using UnityEngine.Experimental.Rendering.Universal;
using System.Collections;
using System.Collections.Generic;

public class TransitionLanternController : MonoBehaviour
{
    [SerializeField]
    private List<LanternState> lanternStates;
    private Light2D lanternLight;
    private SpriteRenderer spriteRenderer;
    private void Awake() {
        lanternLight = GetComponentInChildren<Light2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start() {
        var currentState =  GameController.Instance.currentLevel;
        StartCoroutine(UpdateLanternState(currentState));
    }

    private IEnumerator UpdateLanternState(int state)
    {
        spriteRenderer.sprite = lanternStates[state].sprite;
        while(!lanternLight.color.Equals(lanternStates[state].lightColor))
        {
            var t = Time.time;
            lanternLight.pointLightOuterRadius = Mathf.Lerp(lanternLight.pointLightOuterRadius, lanternStates[state].lightRadius, Mathf.PingPong(t, 1));
            lanternLight.color = Color.Lerp(lanternLight.color, lanternStates[state].lightColor, Mathf.PingPong(t, 1));
            yield return new WaitForFixedUpdate();
        }
    }
}

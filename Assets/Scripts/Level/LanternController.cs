using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System.Collections;
using System.Collections.Generic;

public class LanternController : MonoBehaviour {
    [SerializeField]
    private List<LanternState> lanternStates;
    private Light2D lanternLight;
    private SpriteRenderer spriteRenderer;
    private int currentState;
    private void Awake() {
        lanternLight = GetComponentInChildren<Light2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start() {
        StartCoroutine(UpdateLanternState(currentState));
    }

    public float UseLantern()
    {
        if (currentState.Equals(lanternStates.Count - 1)) {
            return 0;
        }
        var recoverAmount = lanternStates[currentState].recoverAmount;
        currentState = Mathf.Min(currentState + 1, lanternStates.Count - 1);
        StartCoroutine(UpdateLanternState(currentState));
        return recoverAmount;
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

[System.Serializable]
public class LanternState
{
    public Sprite sprite;
    public Color lightColor;
    public float lightRadius;
    [Range(0, 100)]
    public float recoverAmount;
}
using UnityEngine;
using TMPro;
using UnityEngine.Experimental.Rendering.Universal;
using System.Collections;
using System.Collections.Generic;

public class LanternController : MonoBehaviour {
    [SerializeField]
    private List<LanternState> lanternStates;
    [SerializeField]
    private TMP_Text DialogueText;
    [SerializeField]
    private TMP_Text InstructionText;
    [SerializeField]
    [Range(0, 1)]
    private float fadeFactorization;
    private Light2D lanternLight;
    private SpriteRenderer spriteRenderer;
    private int currentState;
    private void Awake() {
        lanternLight = GetComponentInChildren<Light2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        DialogueText.alpha = 0;
        InstructionText.alpha = 0;
    }

    private void Start() {
        StartCoroutine(UpdateLanternState(currentState));
    }

    public float UseLantern()
    {
        var recoverAmount = lanternStates[currentState].recoverAmount;
        currentState = Mathf.Min(currentState + 1, lanternStates.LastIndex());
        if (currentState.Equals(lanternStates.LastIndex())) {
            StartCoroutine(FadeOutText(InstructionText));
            recoverAmount = 0;
        }
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

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag.Equals("Player") && currentState < lanternStates.LastIndex())
        {
            StartCoroutine(FadeInText(InstructionText));
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag.Equals("Player") && currentState < lanternStates.LastIndex())
        {
            StartCoroutine(FadeOutText(InstructionText));
        }
    }

    private IEnumerator FadeInText(TMP_Text text)
    {
        float t = 0;
        while(text.alpha < 1)
        {
            t += Time.deltaTime * fadeFactorization;
            text.alpha = Mathf.Lerp(0, 1, t);
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator FadeOutText(TMP_Text text)
    {
        float t = 0;
        while(text.alpha > 0)
        {
            t += Time.deltaTime * fadeFactorization;
            text.alpha = Mathf.Lerp(1, 0, t);
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
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class EndLanternController : MonoBehaviour
{
    public AudioSource audioSource;
    [SerializeField]
    private EndController endController;
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

    public void UseLantern()
    {
        if (currentState.Equals(lanternStates.LastIndex())) return;

        audioSource.pitch = 0.4f + (0.2f * currentState);
        audioSource.Play();
        currentState = Mathf.Min(currentState + 1, lanternStates.LastIndex());
        if (currentState.Equals(lanternStates.LastIndex())) {
            StartCoroutine(FadeOutText(InstructionText));
            endController.StartCredit();
        }
        StartCoroutine(UpdateLanternState(currentState));
    }

    private IEnumerator UpdateLanternState(int state)
    {
        spriteRenderer.sprite = lanternStates[state].sprite;
        var step = Time.deltaTime;
        while(!lanternLight.color.Equals(lanternStates[state].lightColor) &&
            lanternLight.pointLightOuterRadius < lanternStates[state].lightRadius)
        {
            lanternLight.pointLightOuterRadius = Mathf.Lerp(lanternLight.pointLightOuterRadius, lanternStates[state].lightRadius, step);

            var nextR = Mathf.Lerp(lanternLight.color.r, lanternStates[state].lightColor.r, step);
            var nextG = Mathf.Lerp(lanternLight.color.g, lanternStates[state].lightColor.g, step);
            var nextB = Mathf.Lerp(lanternLight.color.b, lanternStates[state].lightColor.b, step);
            lanternLight.color = new Color(nextR, nextG, nextB, lanternStates[state].lightColor.a);
            step += Time.deltaTime * 0.5f;
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
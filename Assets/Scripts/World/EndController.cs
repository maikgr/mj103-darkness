using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class EndController : MonoBehaviour
{
    public string startScene;
    public AudioSource endingAudio;
    public List<TMP_Text> messages;
    public float fadeDuration;
    public float displayDuration;
    private AnimationCurve creditTextCurve;
    private float animationDelta = -1f;
    private int currentMessageIndex = 0;

    private void Start() {
        ScreenFadeController.Instance.FadeOutScreen(
            3f,
            0,
            Color.black
        );
        creditTextCurve = new AnimationCurve(
            new Keyframe(0f, 0),
            new Keyframe(fadeDuration, 1f),
            new Keyframe(fadeDuration + displayDuration, 1f),
            new Keyframe(fadeDuration + displayDuration + fadeDuration, 0f)
        );
    }

    private void Update() {
        if (animationDelta > -1)
        {
            animationDelta += Time.deltaTime;
            messages[currentMessageIndex].alpha = creditTextCurve.Evaluate(animationDelta);

            if (animationDelta > creditTextCurve[creditTextCurve.length - 1].time)
            {
                animationDelta = -1f;
                var nextIndex = currentMessageIndex + 1;
                if (nextIndex < messages.Count)
                {
                    currentMessageIndex = nextIndex;
                    animationDelta = 0f;
                }
                else {
                    ScreenFadeController.Instance.FadeColorScreen(
                        5f,
                        0,
                        Color.white,
                        Color.black,
                        () => SceneManager.LoadScene(startScene)
                    );
                }
            }
        }
    }

    public void StartCredit()
    {
        endingAudio.PlayDelayed(1f);
        ScreenFadeController.Instance.FadeInScreen(5f, 0, new Color(1, 1, 1, 0), () => {
            animationDelta = 0f;
        });
    }

    private IEnumerator FadeInText(TMP_Text text)
    {
        float t = 0;
        while(text.alpha < 1)
        {
            t += Time.deltaTime;
            text.alpha = Mathf.Lerp(0, 1, t);
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator FadeOutText(TMP_Text text)
    {
        float t = 0;
        while(text.alpha > 0)
        {
            t += Time.deltaTime;
            text.alpha = Mathf.Lerp(1, 0, t);
            yield return new WaitForFixedUpdate();
        }
    }
}

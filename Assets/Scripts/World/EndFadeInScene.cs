using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndFadeInScene : MonoBehaviour {
    public Image Screen;
    public string startScene;

    public void StartEndScene() {
        StopAllCoroutines();
        StartCoroutine(EndSceneFade());
    }

    private IEnumerator EndSceneFade()
    {
        var step = Time.deltaTime * 0.05f;
        while (Screen.color.a < 1)
        {
            var newAlpha = Mathf.MoveTowards(Screen.color.a, 1, step);
            Screen.color = new Color(1, 1, 1, newAlpha);
            step += Time.deltaTime * 0.05f;
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(2f);
        step = Time.deltaTime * 0.2f;
        while (Screen.color.r > 0)
        {
            var newRgb = Mathf.MoveTowards(1, 0, step);
            Screen.color = new Color(newRgb, newRgb, newRgb, 1);
            step += Time.deltaTime * 0.2f;
            yield return new WaitForFixedUpdate();
        }

        SceneManager.LoadScene(startScene);
    }
}
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System.Collections;

public class PlayerLightHurt : MonoBehaviour
{
  public Light2D stableLight;
  public Light2D flickeringLight;
  public float hurtOffset;
  public Color HurtColor;
  private float baseOuterRadius;

  private void OnEnable()
  {
    flickeringLight.color = HurtColor;
    baseOuterRadius = stableLight.pointLightOuterRadius;
    stableLight.pointLightOuterRadius -= hurtOffset;
    CameraShakeController.Shake(0.5f, 0.02f);
    StartCoroutine(LightColorChangeCoroutine());
  }

  private IEnumerator LightColorChangeCoroutine()
  {
    while (flickeringLight.color.g < Color.white.g && flickeringLight.color.b < Color.white.b)
    {
      var deltaTime = Time.fixedDeltaTime;
      flickeringLight.color = Color.Lerp(flickeringLight.color, Color.white, Mathf.PingPong(Time.time, 1));
      stableLight.pointLightOuterRadius = Mathf.Min(stableLight.pointLightOuterRadius + deltaTime, baseOuterRadius);
      yield return new WaitForFixedUpdate();
    }
    flickeringLight.color = Color.white;
    GetComponent<PlayerLightHurt>().enabled = false;
  }
}
using UnityEngine;

public class PlayerAnimationScriptController : MonoBehaviour {
  public PlayerLightFlicker PlayerLightFlicker;
  public PlayerLightHurt PlayerLightHurt;
  private void Start() {
    PlayerLightFlicker.enabled = false;
    PlayerLightHurt.enabled = false;
  }
  public void PlayAnimation(PlayerAnimationName animationName)
  {
    switch(animationName)
    {
      case PlayerAnimationName.LightFlicker:
        PlayerLightFlicker.enabled = true;
        break;
      case PlayerAnimationName.LightHurt:
        PlayerLightHurt.enabled = true;
        break;
    }
  }
  public void StopAnimation(PlayerAnimationName animationName)
  {
    switch(animationName)
    {
      case PlayerAnimationName.LightFlicker:
        PlayerLightFlicker.enabled = false;
        break;
      case PlayerAnimationName.LightHurt:
        PlayerLightHurt.enabled = false;
        break;
    }
  }
}

public enum PlayerAnimationName
{
  LightFlicker = 0,
  PushBack = 1,
  LightHurt = 2
}
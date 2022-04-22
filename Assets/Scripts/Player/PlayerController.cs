using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Light2D stableLight;
    public PlayerAnimationScriptController AnimationScriptController;
    private LevelGenerator levelGenerator;
    private float baseOuterRadius;
    private bool canControl = true;

    // Start is called before the first frame update
    void Start()
    {
        levelGenerator = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelGenerator>();
        baseOuterRadius = stableLight.pointLightOuterRadius + 0;
        AnimationScriptController.PlayAnimation(PlayerAnimationName.LightFlicker);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Movement
        if (canControl) {
            transform.localPosition = new Vector2(
                    transform.localPosition.x + Input.GetAxis("Horizontal") * Time.deltaTime * speed,
                    transform.localPosition.y + Input.GetAxis("Vertical") * Time.deltaTime * speed
                );
        }

        // Light level
        if (Input.GetButton("Fire1"))
        {
            stableLight.pointLightOuterRadius += Time.deltaTime * 5;
        }
        else if (Input.GetButton("Fire2"))
        {
            stableLight.pointLightOuterRadius -= Time.deltaTime * 5;
        }

        if (Input.GetButton("Jump") && !isHurtFlicker)
        {
            isHurtFlicker = true;
            flickeringLight.color = new Color32(219, 47, 47, 255);
            baseOuterRadius = stableLight.pointLightOuterRadius;
            stableLight.pointLightOuterRadius -= 0.5f;
            CameraShakeController.Shake(0.5f, 0.02f);
        }

        if (isHurtFlicker)
        {
            var deltaTime = Time.fixedDeltaTime;
            flickeringLight.color = new Color(flickeringLight.color.r + deltaTime, flickeringLight.color.g + deltaTime, flickeringLight.color.b + deltaTime);
            stableLight.pointLightOuterRadius = Mathf.Min(stableLight.pointLightOuterRadius + deltaTime, baseOuterRadius);
            if (flickeringLight.color.g >= 1f)
            {
                flickeringLight.color = Color.white;
                stableLight.pointLightOuterRadius = baseOuterRadius;
                isHurtFlicker = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "ExitTile")
        {
            levelGenerator.RestartLevel();
        }

        if (other.tag == "Enemy")
        {
            StartCoroutine(PushPlayerBack(other.transform.position));
        }
    }

    private IEnumerator PushPlayerBack(Vector3 otherPos)
    {
        canControl = false;
        var difference = otherPos - this.transform.position;
        this.transform.localPosition = new Vector2(
            transform.localPosition.x - difference.x * 0.5f,
            transform.localPosition.y - difference.y * 0.5f
        );
        yield return new WaitForSeconds(0.3f);
        canControl = true;
    }
}

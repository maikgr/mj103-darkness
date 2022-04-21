using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float flickerOffset;
    public Light2D stableLight;
    public Light2D flickeringLight;
    private LevelGenerator levelGenerator;
    private float baseOuterRadius;
    private float baseInnerRadius;

    // Start is called before the first frame update
    void Start()
    {
        levelGenerator = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelGenerator>();
        baseOuterRadius = stableLight.pointLightOuterRadius + 0;
        baseInnerRadius = stableLight.pointLightInnerRadius + 0;
        StartCoroutine(LightFlickerCoroutine());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Movement
        transform.localPosition = new Vector2(
                transform.localPosition.x + Input.GetAxis("Horizontal") * Time.deltaTime * speed,
                transform.localPosition.y + Input.GetAxis("Vertical") * Time.deltaTime * speed
            );

        // Light level
        if (Input.GetButton("Fire1"))
        {
            stableLight.pointLightOuterRadius += Time.deltaTime * 5;
            baseOuterRadius = stableLight.pointLightOuterRadius + 0;
        }
        else if (Input.GetButton("Fire2"))
        {
            stableLight.pointLightOuterRadius -= Time.deltaTime * 5;
            baseOuterRadius = stableLight.pointLightOuterRadius + 0;
        }
    }

    private IEnumerator LightFlickerCoroutine() {
        // Light flickers
        while(true)
            {
            flickeringLight.pointLightOuterRadius = baseOuterRadius - Random.Range(0f, flickerOffset);
            flickeringLight.pointLightInnerRadius = baseInnerRadius - Random.Range(0f, flickerOffset * 3);
            yield return new WaitForSeconds(Random.Range(0f, 0.2f));
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.tag == "ExitTile")
        {
            levelGenerator.RestartLevel();
        }
    }
}

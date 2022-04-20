using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Light2D light2dController;
    private LevelGenerator levelGenerator;

    // Start is called before the first frame update
    void Start()
    {
        levelGenerator = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelGenerator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.localPosition = new Vector2(
                transform.localPosition.x + Input.GetAxis("Horizontal") * Time.deltaTime * speed,
                transform.localPosition.y + Input.GetAxis("Vertical") * Time.deltaTime * speed
            );
        if (Input.GetButton("Fire1"))
        {
            light2dController.pointLightOuterRadius += Time.deltaTime * 5;
        }
        else if (Input.GetButton("Fire2"))
        {
            light2dController.pointLightOuterRadius -= Time.deltaTime * 5;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.tag == "ExitTile")
        {
            levelGenerator.RestartLevel();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAudioController : MonoBehaviour
{
    [SerializeField]
    private AudioSource StartBgm;

    private void Start()
    {
        StartBgm.Play();
    }

    public void StopBgm()
    {
        StartBgm.Stop();
    }
}

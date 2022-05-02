using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScreenAudioController : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> startBgm;
    [SerializeField]
    private AudioSource startBgmSource;
    [SerializeField]
    private AudioSource mainAudioBgm;
    public float mainAudioDucking;
    private float initialMainBgmVolume;

    private void Awake() {
        var clipIndex = Mathf.Clamp(GameController.Instance.currentLevel, 0, startBgm.LastIndex());
        startBgmSource.clip = startBgm[clipIndex];
        startBgmSource.Play();
        initialMainBgmVolume = mainAudioBgm.volume;
    }

    private void Start() {
        mainAudioBgm.Play();
    }

    private void Update() {
        if (!startBgmSource.isPlaying && mainAudioBgm.volume == mainAudioDucking)
        {
            mainAudioBgm.volume = initialMainBgmVolume;
        }
        else if (startBgmSource.isPlaying && mainAudioBgm.volume != mainAudioDucking)
        {
            mainAudioBgm.volume = mainAudioDucking;
        }
    }
}

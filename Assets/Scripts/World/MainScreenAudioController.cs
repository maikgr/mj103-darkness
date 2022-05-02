using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScreenAudioController : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> AudioClips;
    [SerializeField]
    private AudioSource AudioSource;

    private void Awake() {
        var clipIndex = Mathf.Clamp(GameController.Instance.currentLevel, 0, AudioClips.LastIndex());
        AudioSource.clip = AudioClips[clipIndex];
        AudioSource.Play();
    }
}

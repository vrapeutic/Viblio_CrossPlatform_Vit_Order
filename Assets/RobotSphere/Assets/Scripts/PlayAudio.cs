using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    AudioSource audioSourse;

    private void Start()
    {
        audioSourse = GetComponent<AudioSource>();
    }

    public void Play_Audio(AudioClip clip)
    {
        audioSourse.clip = clip;
        audioSourse.Play();
    }
}

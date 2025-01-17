using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioSourceMultiLanguage : MonoBehaviour
{
    [SerializeField] BoolValue vitLanguage;
    [SerializeField] AudioClip engAudioCliip;
    [SerializeField] AudioClip vitAudioCliip;

    public void PlaySound()
    {
        if (vitLanguage.Value) GetComponent<AudioSource>().PlayOneShot(vitAudioCliip);
        else GetComponent<AudioSource>().PlayOneShot(engAudioCliip);
    }
}

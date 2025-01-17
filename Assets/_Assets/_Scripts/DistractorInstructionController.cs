using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractorInstructionController : MonoBehaviour
{
    bool canPlayInstruction = false;
    [SerializeField] float timeToWaitFirstInstruction ;
    [SerializeField] BoolValue vitLanguage;
    [SerializeField] AudioClip engAudioCliip;
    [SerializeField] AudioClip vitAudioCliip;
    public void PlayInstruction()
    {
        canPlayInstruction = true;
        StartCoroutine(PlayInstructionIEnum());
    }

    IEnumerator PlayInstructionIEnum()
    {
        yield return new WaitForSeconds(timeToWaitFirstInstruction);

        while (canPlayInstruction)
        {
            if (vitLanguage.Value) GetComponent<AudioSource>().PlayOneShot(vitAudioCliip);
            else GetComponent<AudioSource>().PlayOneShot(engAudioCliip);
            yield return new WaitForSeconds(15);
        }
    }

    public void StopPlayingInstraction()
    {
        canPlayInstruction = false;
        GetComponent<AudioSource>().Stop();
    }
}

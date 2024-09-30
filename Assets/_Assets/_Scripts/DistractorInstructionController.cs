using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractorInstructionController : MonoBehaviour
{
    bool canPlayInstruction = false;
    [SerializeField] float timeToWaitFirstInstruction ;
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
            GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(15);
        }
    }

    public void StopPlayingInstraction()
    {
        canPlayInstruction = false;
        GetComponent<AudioSource>().Stop();
    }
}

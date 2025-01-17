using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSoundController : MonoBehaviour
{
    //vit sounds
    [SerializeField]
    AudioClip[] Level1NPCIntroandEnd;
    [SerializeField]
    AudioClip[] Level2NPCIntroVit;
    [SerializeField]
    AudioClip[] Instructions;
    //english sounds
    [SerializeField]
    AudioClip[] Level1NPCIntroandEndEng;
    [SerializeField]
    AudioClip[] Level2NPCIntroEng;
    [SerializeField]
    AudioClip[] InstructionsEng;
    AudioSource speaker;
    [SerializeField]BoolValue isVit ;
    // Start is called before the first frame update
    void Start()
    {
        speaker = GetComponent<AudioSource>();
    }

    public void PlayLevel1Sound(int id)
    {
        if (isVit.Value) speaker.clip = Level1NPCIntroandEnd[id];
        else speaker.clip = Level1NPCIntroandEndEng[id]; ;
        speaker.Play();
    }

    public void PlayLevel2Sound(int id)
    {
        if (isVit.Value) speaker.clip = Level2NPCIntroVit[id];
        else speaker.clip = Level2NPCIntroEng[id];
        speaker.Play();
    }

    public void PlayInstructions(int id)
    {
        if(isVit.Value) speaker.clip = Instructions[id];
        else speaker.clip = InstructionsEng[id];
        speaker.Play();
    }

    public void StopSound()
    {
        speaker.Stop();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

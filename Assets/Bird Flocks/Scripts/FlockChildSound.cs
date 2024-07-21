using Tachyon;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class FlockChildSound:MonoBehaviour
{
    //public AudioClip[] _idleSounds;
    //public float _idleSoundRandomChance = .05f;
    //vit sounds
    public AudioClip[] _flightSounds;
    //english sounds
    public AudioClip[] _flightSoundsEng;
    public float _flightSoundRandomChance = .05f;


    //public AudioClip[] _scareSounds;
    public float _pitchMin = .85f;
    public float _pitchMax = 1.0f;

    public float _volumeMin = .6f;
    public float _volumeMax = .8f;
    int clipNo;
    float pitchValue; 
    float volume;
    //FlockChild _flockChild;
    AudioSource _audio;
    //bool _hasLanded;
    bool isVit = false;
    public void Start()
    {
        if (Statistics.instance.languageIndex == 1) isVit=true;
        this.gameObject.name = "Bird1";
        _audio = GetComponent<AudioSource>();
        InvokationManager invokationManager = new InvokationManager(this, this.gameObject.name);
        NetworkManager.InvokeClientMethod("PlayAudioRPC", invokationManager);
        //_flockChild = GetComponent<FlockChild>();
    	if(Statistics.instance.android)    InvokeRepeating("PlayRandomSound", Random.value+1, 4f);
        //if(_scareSounds.Length > 0)
        //InvokeRepeating("ScareSound", 1.0f, .01f);
    }
    
    public void PlayRandomSound() {
    	if(gameObject.activeInHierarchy)
        {
            if (!_audio.isPlaying && _flightSounds.Length > 0 && _flightSoundRandomChance > Random.value)
            {// && !_flockChild._landing){
                int clipNo = Random.Range(0, _flightSounds.Length);
                float pitchValue = Random.Range(_pitchMin, _pitchMax);
                float volume = Random.Range(_volumeMin, _volumeMax);
                NetworkManager.InvokeServerMethod("PlayAudioRPC", this.gameObject.name, clipNo, pitchValue, volume);
            }
        }
    }

    public void PlayAudioRPC(int _clipNo, float _pitchValue,float _volume)
    {
        if (isVit) _audio.clip = _flightSounds[_clipNo];
        else _audio.clip = _flightSoundsEng[_clipNo];
        _audio.pitch = _pitchValue;
        _audio.volume = _volume;
        _audio.Play();
    }
    
    //public void ScareSound() {	
    //if(gameObject.activeInHierarchy){
    //	if(_hasLanded && !_flockChild._landing && _idleSoundRandomChance*2 > Random.value){
    //		_audio.clip = _scareSounds[Random.Range(0,_scareSounds.Length)];
    //		_audio.volume = Random.Range(_volumeMin, _volumeMax);
    //		_audio.PlayDelayed(Random.value*.2f);
    //		_hasLanded = false;
    //	}
    //	}
    //}
}

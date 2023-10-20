using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BoundaryDetection : MonoBehaviour
{
    [SerializeField] AudioClip audioClip;
    Transform _camera;
    AudioSource audioSourse;
    WaitForSeconds aPartSecond;
    Vector3 newPosition;
    bool notFocused;
    bool canCheck = true;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_ANDROID
        //Debug.Log("Start");
        _camera = Camera.main.transform;
        aPartSecond = new WaitForSeconds(.2f);
        newPosition = new Vector3();
        audioSourse = GetComponent<AudioSource>();
        audioSourse.playOnAwake = false;
        audioSourse.clip = audioClip;
        audioSourse.playOnAwake = false;
        CheckDetector();
        #endif
    }
    

    async void CheckDetector()
    {
/**/ /*
        while (canCheck)
        {
          if (OVRManager.boundary.GetVisible() && notFocused)
            {
                if (!audioSourse.isPlaying)
                {
                    audioSourse.Play();
                    SetAudioSourcePosition();
                    OVRInput.SetControllerVibration(1, 0.7f, OVRInput.Controller.RTouch);
                    OVRInput.SetControllerVibration(1, 0.7f, OVRInput.Controller.LTouch);
                }
            }
            else
            {
                audioSourse.Stop();
                OVRInput.SetControllerVibration(1, 0, OVRInput.Controller.RTouch);
                OVRInput.SetControllerVibration(1, 0, OVRInput.Controller.LTouch);
            }
            await aPartSecond;
        }
    */
    }

    private void SetAudioSourcePosition()
    {
        newPosition.x = _camera.position.x;
        newPosition.y = _camera.position.y;
        newPosition.z = _camera.position.z + 1;
        transform.position = newPosition;
    }

    async void HMDMountedFunc()
    {
        await avoidOnFocusSound();
    }

    async Task avoidOnFocusSound()
    {
        notFocused = false;
        await new  WaitForSeconds(5);
        notFocused = true;
    }

/*    private void OnEnable()
    { 
        OVRManager.HMDMounted += HMDMountedFunc;
    }

    private void OnDisable()
    {
        OVRManager.HMDMounted -= HMDMountedFunc;
        canCheck = false;
    }
*/
}

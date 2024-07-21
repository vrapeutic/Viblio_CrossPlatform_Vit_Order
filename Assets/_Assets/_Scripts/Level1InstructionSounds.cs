using System.Collections;
using System.Collections.Generic;
using Tachyon;
using UnityEngine;

public class Level1InstructionSounds : MonoBehaviour
{
    bool bookHold;
    bool bookPut;
    bool routineCanWork= true;
    WaitForSeconds waitedTime;
    NPCSoundController speaker;
    IEnumerator CheckIfBookHoldRoutine;
    IEnumerator CheckIfBookPutRoutine;
    // Start is called before the first frame update
    void Start()
    {
        InvokationManager invokationManager = new InvokationManager(this, this.gameObject.name);
        NetworkManager.InvokeClientMethod("PlayInstructionRPC", invokationManager);
        speaker = FindObjectOfType<NPCSoundController>();
        waitedTime = new WaitForSeconds(25);
    }

    private void OnEnable()
    {
        routineCanWork = true;
        if (!Statistics.instance.android) return;
        /**/ //OVRGrabber.OnGrabBegin += OnGrabBeginFunc;
     /**/ //    OVRGrabber.OnGrabEndWithGrabbable += OnGrabEndFunc;
        GameManager.OnLevelBegin += CheckIfBookHold;
        GameManager.OnLevelEnd += CloseAllIEnumarators;
    }

    private void OnDisable()
    {
        if (!Statistics.instance.android) return;
        /**/ //OVRGrabber.OnGrabBegin -= OnGrabBeginFunc;
        /**/ // OVRGrabber.OnGrabEndWithGrabbable -= OnGrabEndFunc;
        GameManager.OnLevelBegin -= CheckIfBookHold;
        GameManager.OnLevelEnd -= CloseAllIEnumarators;
    }

    public void OnGrabBeginFunc()
    {
        bookHold = true;
        CheckIfBookPut();
    }

    public void OnGrabEndFunc()
    {
        bookPut = true;
        CheckIfBookHold();
    }


    void CheckIfBookHold()
    {
        if (!GameManager.instance.currentlyPlaying) return;
        bookHold = false;
        if (CheckIfBookHoldRoutine != null)
            StopCoroutine(CheckIfBookHoldRoutine);
        CheckIfBookHoldRoutine = CheckIfBookHoldInum();
        StartCoroutine(CheckIfBookHoldRoutine);
    }

    public IEnumerator CheckIfBookHoldInum()
    {
        while (routineCanWork && GameManager.instance.currentlyPlaying)
        {
            yield return waitedTime;

            if (bookHold)
            {
                break;
            }
            NetworkManager.InvokeServerMethod("PlayInstructionRPC", this.gameObject.name, 4);
        }
    }

    void CheckIfBookPut()
    {
        if (!GameManager.instance.currentlyPlaying) return;
        bookPut = false;
        if (CheckIfBookPutRoutine != null)
            StopCoroutine(CheckIfBookPutRoutine);
        CheckIfBookPutRoutine = CheckIfBookPutInum();
        StartCoroutine(CheckIfBookPutRoutine);
    }

    public IEnumerator CheckIfBookPutInum()
    {
        while (routineCanWork && GameManager.instance.currentlyPlaying)
        {
            yield return waitedTime;

            if (bookPut)
            {
                break;
            }
            NetworkManager.InvokeServerMethod("PlayInstructionRPC", this.gameObject.name, 5);
        }
    }

    public void PlayInstructionRPC(int id)
    {
        speaker.PlayInstructions(id);
    }

    void CloseAllIEnumarators()
    {
        routineCanWork = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventsHandler : MonoBehaviour
{

    public UnityEvent[] animationEvents;

    public void HandleEvent(int eventKey)
    {
        if (eventKey >= animationEvents.Length)
        {
            //Debug.LogFormat("Unavailable Animation Event for the Event Key");
            return;
        }


        if (animationEvents[eventKey] != null)
        {
            animationEvents[eventKey].Invoke();
        }


    }
}

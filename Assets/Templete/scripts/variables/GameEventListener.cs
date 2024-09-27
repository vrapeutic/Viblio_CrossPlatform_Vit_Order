using System.Collections;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")]
    public GameEvent Event;

    [Tooltip("Response to invoke when Event is raised.")]
    public UnityEvent Response;
    [Tooltip("If you want to put delayon Response")]

    [SerializeField] float timeToWait = 0;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        try
        {
            if (timeToWait == 0)
                Response.Invoke();
            else
                StartCoroutine(OnEventRaisedIenum());
        }
        catch
        {
            Debug.LogError("null ref On " + gameObject.name);
        }
    }

    IEnumerator OnEventRaisedIenum()
    {
        yield return new WaitForSeconds(timeToWait);
        Response.Invoke();
    }
}

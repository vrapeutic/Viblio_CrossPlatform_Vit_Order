using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MultiGameEventListener : MonoBehaviour
{
    public  GameEvent [] Events;
    public UnityEvent Response;
    [SerializeField] float timeToWait = 0;
    private void OnEnable()
    {
        foreach (var item in Events)
        {
            item.RegisterListener(this);
        }
    }
    
    private void OnDisable()
    {
        foreach (var item in Events)
        {
            item.UnregisterListener(this);
        }
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

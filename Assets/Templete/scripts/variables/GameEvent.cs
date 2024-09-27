using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    /// <summary>
    /// The list of listeners that this event will notify if it is raised.
    /// </summary>
    private readonly List<GameEventListener> eventListeners =
        new List<GameEventListener>();
    private readonly List<MultiGameEventListener> multiEventListeners =
    new List<MultiGameEventListener>();

    public void Raise()
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
            eventListeners[i].OnEventRaised();
        for (int i = multiEventListeners.Count - 1; i >= 0; i--)
            multiEventListeners[i].OnEventRaised();
        Debug.Log(this.name);
    }

    public void RegisterListener(GameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
    public void RegisterListener(MultiGameEventListener multiListener)
    {
        if (!multiEventListeners.Contains(multiListener))
            multiEventListeners.Add(multiListener);
    }

    public void UnregisterListener(MultiGameEventListener multiListener)
    {
        if (multiEventListeners.Contains(multiListener))
            multiEventListeners.Remove(multiListener);
    }

}

using System.Data;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using Tachyon;
public class LocalEventManager 
{

    public static Dictionary<string, List<Action<SocketIOEvent>>> globalHandlers = new Dictionary<string, List<Action<SocketIOEvent>>>();

    public static void On(string ev, InvokationManager invokationManager)
    {
        Action<SocketIOEvent> callback = invokationManager.InvokeFunction;
        if (globalHandlers.ContainsKey(ev))
        {
            globalHandlers.Remove(ev);
            //.Log("key removed");
        }
            globalHandlers[ev] = new List<Action<SocketIOEvent>>();
            globalHandlers[ev].Add(callback);
        
    }

    public static void InvokeLocalEvent(string fnName, JSONObject data)
    {
        if (!globalHandlers.ContainsKey(fnName))
        {
            return;
        }
        SocketIOEvent ev = new SocketIOEvent(fnName, data);
        foreach (Action<SocketIOEvent> handler in globalHandlers[fnName])
        {
            handler.Invoke(ev);
        }
    }

}

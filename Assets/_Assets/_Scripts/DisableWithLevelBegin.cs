using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tachyon;

public class DisableWithLevelBegin : MonoBehaviour
{

    private void Start()
    {
        InvokationManager invokationManager = new InvokationManager(this, this.gameObject.name);
        NetworkManager.InvokeClientMethod("DisableThisGameObjectRPC", invokationManager);
    }

    private void OnEnable()
    {
        GameManager.OnLevelBegin += DisableThisGameObject;
    }

    private void OnDisable()
    {
        GameManager.OnLevelBegin -= DisableThisGameObject;
    }

    private void DisableThisGameObject()
    {
        //this.gameObject.SetActive(false);
        NetworkManager.InvokeServerMethod("DisableThisGameObjectRPC", this.gameObject.name);
    }

    public void DisableThisGameObjectRPC()
    {
        this.gameObject.SetActive(false);
    }
}

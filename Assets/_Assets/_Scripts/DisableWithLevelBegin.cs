using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableWithLevelBegin : MonoBehaviour
{

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
        this.gameObject.SetActive(false);
    }
}

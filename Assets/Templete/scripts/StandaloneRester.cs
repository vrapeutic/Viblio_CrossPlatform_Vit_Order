using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tachyon;


public class StandaloneRester : MonoBehaviour
{
    [SerializeField] BoolValue isStandalone;
    // Start is called before the first frame update
    void Awake()
    {
        isStandalone.Value = false;
    }

    void Start()
    {
        NetworkManager.SetStandaloneValue(isStandalone.Value);
    }

}

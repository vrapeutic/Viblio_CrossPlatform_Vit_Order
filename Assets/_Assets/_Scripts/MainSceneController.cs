using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(Statistics.instance.android)
            FindObjectOfType<Robot>().Open();
        Statistics.instance.OnStart();
    }
}

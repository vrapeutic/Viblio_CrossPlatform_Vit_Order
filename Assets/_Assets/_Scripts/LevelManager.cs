using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private void Awake()
    {
        if (Statistics.instance.level == 1)
        {
            gameObject.AddComponent<Level1Controller>();
        }
        else if (Statistics.instance.level == 2)
            gameObject.AddComponent<Level2Controller>();
        else gameObject.AddComponent<Level3Controller>();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactiveGameObjectDesktop : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        #if UNITY_STANDALONE
         gameObject.SetActive(false);
    
        #endif
    }
}

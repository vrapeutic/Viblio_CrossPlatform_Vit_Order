using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class DesktopCamera : MonoBehaviour
{
    Transform targetObjetc;
    Vector3 newPos = new Vector3();

    // Start is called before the first frame update
    void OnEnable()
    {
#if UNITY_ANDROID
        targetObjetc = GameObject.FindGameObjectWithTag("MainCamera").transform;
        GetComponent<Camera>().enabled = false;
#else
            GetComponent<Camera>().depth = 1;
            gameObject.AddComponent<AudioListener>();
#endif

    }

    private void Update()
    {
#if UNITY_ANDROID
            newPos.x = targetObjetc.position.x;
            newPos.y = targetObjetc.position.y;
            newPos.z = targetObjetc.position.z;
            transform.position = newPos;
            transform.rotation = targetObjetc.rotation;
        
#endif
}
}

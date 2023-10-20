using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    Transform VRCameraPosition;
    Vector3 newPos = new Vector3();

    //private void Awake()
    //{
    //    if (!Statistics.instance.android) this.enabled = false;
    //}

    void Start()
    {
        if (Statistics.instance.android)
        {
            VRCameraPosition = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }
        else
        {
            VRCameraPosition = GameObject.Find("Desktop Camera").transform;
            //Debug.Log("VRCameraPos"+VRCameraPosition);
        }
    }

    private void Update()
    {
            newPos.x = VRCameraPosition.position.x;
            newPos.y = VRCameraPosition.position.y;
            newPos.z = VRCameraPosition.position.z;
            transform.position = newPos;
    }
}

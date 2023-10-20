using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactiveDesktopVRGrabber : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
/**/        if (!Statistics.instance.android) ;
            //GetComponent<OVRGrabber>().enabled = false;
    }

}

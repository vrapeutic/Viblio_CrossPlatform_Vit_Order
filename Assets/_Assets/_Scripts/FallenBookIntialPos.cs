using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenBookIntialPos : MonoBehaviour
{
    Vector3 intialPos;

    // Start is called before the first frame update
    public void SavePos()
    {
        intialPos = transform.localPosition;
        Debug.Log("SavePos()" + intialPos.y);
    }

    private void OnEnable()
    {
        Debug.Log(intialPos.y);
        GetComponent<Rigidbody>().isKinematic = true;
        transform.localPosition = intialPos;
        transform.localRotation = Quaternion.identity;
        GetComponent<Rigidbody>().isKinematic = false;
        //transform.rotation = intialPos.rotation;
    }

}

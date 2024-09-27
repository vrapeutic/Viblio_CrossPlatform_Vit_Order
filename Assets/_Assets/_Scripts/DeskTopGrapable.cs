using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DeskTopGrapable : MonoBehaviour
{
    // Start is called before the first frame update
    XRGrabInteractable xrGrabInteractable;
    XRDirectInteractor rightHandInteractor;
    Rigidbody rb;
    Vector3 pos;
    Quaternion rot;
    Transform leftHandTrans;
    Transform rightHandTrans;

    WaitForSeconds a7partSecond;

    void Start()
    {
        xrGrabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        rightHandInteractor = GameObject.FindGameObjectWithTag("RightHand").GetComponent<XRDirectInteractor>();

        a7partSecond = new WaitForSeconds(.7f);
        pos = new Vector3(0, 0, 0);
        rot = new Quaternion(0, 0, 0, 0);
        leftHandTrans = GameObject.FindGameObjectWithTag("LeftHand").transform;
        rightHandTrans = GameObject.FindGameObjectWithTag("RightHand").transform;
    }



    public void BookGraped()
    {
        //Debug.Log(gameObject.name + "BookGraped ");
        if (xrGrabInteractable.IsSelectableBy(rightHandInteractor)) GrabbedByRightHand();
        else GrabbedByLeftHand();
    }

    public void GrabbedByLeftHand()
    {
        //Debug.Log("GrabbedByLeftHand");
        if (Statistics.instance.android) return;
        transform.SetParent(leftHandTrans);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    public void GrabbedByRightHand()
    {
        //Debug.Log("GrabbedByRightHand");
        if (Statistics.instance.android) return;
        transform.SetParent(rightHandTrans);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    public void BookReleased()
    {
        if (!Statistics.instance.android) return;
        Released();
        StartCoroutine(AfterBookSync());
    }
    public void Released()
    {
        //Debug.Log("Released");
        if (Statistics.instance.android) return;
        rb.isKinematic = false;
        rb.useGravity = true;
        transform.SetParent(null);
    }

    IEnumerator AfterBookSync()
    {
        if (!Statistics.instance.android) yield break;
        yield return a7partSecond;
        AfterBookTransformSyncRPC( transform.position.x, transform.position.y, transform.position.z, transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
    }

    public void AfterBookTransformSyncRPC(float _x, float _y, float _z, float rx, float ry, float rz, float rw)
    {
       // Debug.Log("AfterBookPosSyncRPC");
        if (Statistics.instance.android) return;
        pos.x = _x;
        pos.y = _y;
        pos.z = _z;
        rot.x = rx;
        rot.y = ry;
        rot.z = rz;
        rot.w = rw;
        transform.position = pos;
        transform.rotation = rot;
        StartCoroutine(MakeBookKinametic());
    }

    IEnumerator MakeBookKinametic()
    {
        yield return a7partSecond;
        rb.isKinematic = true;
        rb.useGravity = false;
        transform.position = pos;
        transform.rotation = rot;
    }

}
//GetComponent<XRGrabInteractable>().onSelectEnter += BookGraped;
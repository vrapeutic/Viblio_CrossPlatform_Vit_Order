using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    Animator robotAnim;
    Transform robotHomeBattery;
    Transform levelIntroIntialPosition;
    AudioSource robotSound;


    private void OnEnable()
    {
        transform.position = new Vector3(1,0, 0.582f);
        transform.rotation = new Quaternion(0.0f,.7f,0.0f,-.7f);
        robotAnim = GetComponent<Animator>();
        robotSound= GetComponent<AudioSource>();
    }
    #region animation states
    public void Walk()
    {
        //Debug.Log("walk");
        robotAnim.SetBool("Walk_Anim", true);
    }

    public void Open()
    {
        //Debug.Log("Open");
        robotAnim.SetBool("Open_Anim", true);
        robotAnim.SetBool("Close_Anim", false);
    }

    public void Close()
    {
        //Debug.Log("Close");
        robotAnim.SetBool("Open_Anim", false);
        robotAnim.SetBool("Close_Anim", true);
    }

    public void Idle ()
    {
        //Debug.Log("Idle");
        robotSound.Stop();
        robotAnim.SetTrigger("Idle_Anim");
        robotAnim.SetBool("Roll_Anim", false);
        robotAnim.SetBool("Walk_Anim", false);
        //robotAnim.SetBool("Open_Anim", true);
    }

    public void Roll()
    {
        robotAnim.SetBool("Roll_Anim", true);
    }

    public void OpenEye()
    {
        robotAnim.SetInteger("Open_Part",1);
    }

    public void OpenLeg()
    {
        robotAnim.SetInteger("Open_Part", 2);
    }

    public void CloseEye()
    {
        robotAnim.SetInteger("Open_Part", 0);
    }

    public void CloseLeg()
    {
        robotAnim.SetInteger("Open_Part", 1);
    }

    public void DropDown()
    {
        robotAnim.SetTrigger("DropDown_Anim");
    }

    public void RiseUp()
    {
        robotAnim.SetTrigger("RiseUp_Anim");
    }

    #endregion

    public void SetAtHome()
    {
        robotHomeBattery = GameObject.Find("Battery").transform;
        transform.SetParent(null);
        Close();
        transform.SetParent(robotHomeBattery);
        transform.localPosition = new Vector3(0,1.1f,0);
    }

    public void SetLevelIntroPosition(Transform robotPosition)
    {
        transform.SetParent(null);
        transform.SetParent(robotPosition);
        transform.localPosition =Vector3.zero;
        transform.localRotation = Quaternion.identity;
        //Debug.Log("SetLevelIntroPosition"+ transform.localPosition);
    }
}

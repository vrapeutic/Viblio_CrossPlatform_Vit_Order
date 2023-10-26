using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotDistractingTask : MonoBehaviour
{
    Robot robot;
    HandHider rightHand;
    HandHider leftHand;
    GameObject RobotAnimationPositions;
    RobotSoundController robotSound;
    [SerializeField] GameEvent onRobotNeedAction;
    Coroutine WaitAndBeginDistractingCor;
    // Start is called before the first frame update

    void Start()
    {
        robot = GetComponent<Robot>();
        RobotAnimationPositions = GameObject.Find("RobotAnimations");
        rightHand = GameObject.FindGameObjectWithTag("RightHand").GetComponent<HandHider>();
        leftHand = GameObject.FindGameObjectWithTag("LeftHand").GetComponent<HandHider>();
        robotSound = FindObjectOfType<RobotSoundController>();
        WaitAndBeginDistracting();
    }

    public void WaitAndBeginDistracting()
    {
        StopAllCoroutines();
        StopCoroutine(WaitAndBeginDistractingIenum());
        StartCoroutine(WaitAndBeginDistractingIenum());
    }
    IEnumerator WaitAndBeginDistractingIenum()
    {
        yield return new WaitForSeconds(.2f);
        robotSound.StopSound();
        robot.Open();
        yield return new WaitForSeconds(4.8f);
        RobotAnimationPositions.transform.GetChild(0).localPosition = new Vector3(1.011f, 0, 0.582f);
        RobotAnimationPositions.transform.GetChild(0).localRotation = Quaternion.identity;
        robot.SetLevelIntroPosition(RobotAnimationPositions.transform.GetChild(0));
        //yield return new WaitForSeconds(5);
        BeginRobotDistracting();
    }


    void BeginRobotDistracting()
    {
        StopCoroutine(BeginRobotDistractingIenum());
        StartCoroutine(BeginRobotDistractingIenum());
    }

    IEnumerator BeginRobotDistractingIenum()
    {
        robot.Idle();
        robot.Walk();
        RobotAnimationPositions.GetComponent<Animator>().enabled = true;
        RobotAnimationPositions.GetComponent<Animator>().Play("idle", -1, 0f);
        yield return new WaitForSeconds(30);
        onRobotNeedAction.Raise();
        if (!GameManager.instance.currentlyPlaying) yield break;
        RobotAnimationPositions.GetComponent<Animator>().enabled = false;
        robot.Idle();
        Debug.Log("robot.DropDown();");
        robot.DropDown();
        try
        {
            rightHand.ShowHand(false);
            //Debug.Log("right hand should shown");
            leftHand.ShowHand(false);
            //Debug.Log("left hand should shown");
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
        robotSound.PlayLevel3Sound(6);//08Oops! I guess I need a little  help here
        yield return new WaitForSeconds(5);
        robotSound.PlayLevel3Sound(7);//09Can you please touch my broken leg in the right place
    }

    public void CompleteDistractingTask()
    {
        StopCoroutine(CompleteDistractingTaskIenum());
        StopCoroutine(BeginRobotDistractingIenum());
        StartCoroutine(CompleteDistractingTaskIenum());
    }

    IEnumerator CompleteDistractingTaskIenum()
    {
        robot.RiseUp();
        yield return new WaitForSeconds(3);
        robot.Walk();
        RobotAnimationPositions.SetActive(true);
        RobotAnimationPositions.GetComponent<Animator>().enabled = true;
        try
        {
            if (rightHand.isShown) rightHand.ShowHand(false);
            else rightHand.HideHand(false);
            //Debug.Log("right hand should shown");
            if (leftHand.isShown) leftHand.ShowHand(false);
            else leftHand.HideHand(false);
            //Debug.Log("left hand should shown");

        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }

    public void WaitAndEndDistracting()
    {
        StopCoroutine(WaitAndEndDistractingIenum());
        StartCoroutine(WaitAndEndDistractingIenum());
    }

    IEnumerator WaitAndEndDistractingIenum()
    {
        yield return new WaitForSeconds(10);
        robot.transform.SetParent(null);
        RobotAnimationPositions.GetComponent<Animator>().Play("idle", -1, 0f);
        robot.Idle();
        //await new WaitForSeconds(1);
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
}


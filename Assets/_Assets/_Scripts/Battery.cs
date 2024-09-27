using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    GameObject[] batteryBars;
    NPCSoundController speaker;
    Robot robot;
    int oldState;
    int currentState = 0;

    void Start()
    {
        robot = FindObjectOfType<Robot>();
        batteryBars = new GameObject[6];
        speaker = FindObjectOfType<NPCSoundController>();
        for (int i = 0; i < 6; i++)
        {
            batteryBars[i] = transform.GetChild(i).gameObject;
        }
    }
   

    public void ControlBatteryBarsAndRobotState(int state)
    {

        if (state == oldState) return;
        //**////NetworkManager.InvokeServerMethod("ControlBatteryBarsAndRobotStateRPC", this.gameObject.name, state); 
        ControlBatteryBarsAndRobotStateRPC(state);
        oldState = state;
    }

    public void ControlBatteryBarsAndRobotStateRPC(int state)
    {
        PlaySpeakerSound(state);
        switch (state)
        {
            case 0:
                ActivateBars(0);
                break;
            case 1:
                ActivateBars(1); robot.CloseEye();
                break;
            case 2:
                ActivateBars(2); robot.OpenEye();
                break;
            case 3:
                ActivateBars(3); robot.CloseLeg();
                break;
            case 4:
                ActivateBars(4); robot.OpenLeg();
                break;
            case 5:
                ActivateBars(5);
                break;
            case 6:
                ActivateBars(6);
                break;
        }
    }

    //for the battery is filling up sound
    void PlaySpeakerSound(int state)
    {
        if (state > currentState)
        {
            if (state % 2 == 1)
                speaker.PlayInstructions(3);
            else if (state == 2) speaker.PlayInstructions(0);
            else if (state == 4) speaker.PlayInstructions(1);
        }
        currentState = state;
    }

    void ActivateBars(int activateTillNo)
    {
        for (int i = 0; i < activateTillNo; i++)
        {
            batteryBars[i].SetActive(true);
        }

        for (int i = activateTillNo; i < 6; i++)
        {
            batteryBars[i].SetActive(false);
        }
    }
}




//this deprecated code when we told every color for every bar 
//GameObject[] blueBars;//color code =0
//GameObject[] yellowBars;//color code =1
//GameObject[] redBars;//colorcode =2

//// Start is called before the first frame update
//void Start()
//{
//    blueBars[0] =transform.GetChild(0).gameObject;
//    blueBars[1] = transform.GetChild(1).gameObject;
//    yellowBars[0] =transform.GetChild(2).gameObject;
//    yellowBars[1] = transform.GetChild(3).gameObject;
//    redBars[0] =transform.GetChild(0).gameObject;
//    redBars[1] = transform.GetChild(1).gameObject;
//}
////for states 0 for empty
////           1 for half
////           2 for full

//public void ControlBars(int colorCode,int state)
//{
//    switch (state)
//    {
//        case 0:
//            for (int i = 0; i < 2; i++)
//            {
//                if (colorCode == 0) blueBars[i].SetActive(false);
//                else if (colorCode == 1) yellowBars[i].SetActive(false);
//                else redBars[i].SetActive(false);
//            }
//            break;
//        case 1:
//            for (int i = 0; i < 2; i++)
//            {
//                if (colorCode == 0) { blueBars[0].SetActive(true); blueBars[1].SetActive(true); }
//                else if (colorCode == 1) { yellowBars[0].SetActive(true); yellowBars[1].SetActive(true); }
//                else { redBars[0].SetActive(true); redBars[1].SetActive(true); }
//            }
//            break;
//        case 2:
//            for (int i = 0; i < 2; i++)
//            {
//                if (colorCode == 0) blueBars[i].SetActive(true);
//                else if (colorCode == 1) yellowBars[i].SetActive(true);
//                else redBars[i].SetActive(true);
//            }
//            break;

//    }

//}
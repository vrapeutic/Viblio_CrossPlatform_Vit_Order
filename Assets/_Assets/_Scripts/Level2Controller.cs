using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tachyon;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Level2Controller : MonoBehaviour, ILevel
{
    GameObject books;
    Robot robot;
    GameObject batteryObject;
    Battery battery;
    GameObject bird;
    bool introLevelSkipped=false;
    NPCSoundController speaker;
    #region wait times
    WaitForSeconds a3Seconds;
    WaitForSeconds a5Seconds;
    WaitForSeconds a7Seconds;
    #endregion

    private void Start()
    {
        books = GameObject.Find("Books");
        robot = FindObjectOfType<Robot>();
        batteryObject = GameObject.Find("Battery");
        speaker = FindObjectOfType<NPCSoundController>();
        #region wait times
        a3Seconds = new WaitForSeconds(3);
        a5Seconds = new WaitForSeconds(5);
        a7Seconds = new WaitForSeconds(7);
        #endregion
    }

    public  void BeginLevel()
    {

        if (!introLevelSkipped)
            StartCoroutine(BeginLevel2());
        //GameManager.instance.FireOnLevelBegin();
    }

    IEnumerator BeginLevel2()
    {
        //Debug.Log("BeginLevel2");
        battery = batteryObject.AddComponent<Battery>();
        yield return a3Seconds;
        bird = (GameObject)Instantiate(Resources.Load("Bird"), transform.position, Quaternion.identity);//.name="Bird1";
        bird.GetComponentInChildren<AudioSource>().enabled = false;
        speaker.PlayLevel2Sound(0);
        yield return new WaitForSeconds(11);//10Hello its me again, Adam. Do you remember our friend leo ,His battery died and needs to be charged up
        speaker.PlayLevel2Sound(1);
        yield return a7Seconds;//11Lets organize those books according to their colors to give him a suitable charge.
        speaker.PlayLevel2Sound(2);
        yield return a7Seconds;//12Each Shelf has a specific color, Match the color of each book with the color of the shelf. 
        bird.GetComponentInChildren<AudioSource>().enabled = true;
        GameManager.instance.FireOnLevelBegin();
        //EndLevel();
    }

    public async void EndLevel()
    {
        if (GameManager.instance.successed)
        {
            robot.SetAtHome();
            battery.GetComponent<Animator>().enabled = true;
            battery.GetComponent<Animator>().SetBool("RiseUp", false);
            battery.GetComponent<AudioSource>().Play();
            await new WaitForSeconds(8);
            //Debug.Log("Level2Controller " + " EndLevel "+Statistics.instance.level);
            robot.Open();
            await a3Seconds;
            FindObjectOfType<RobotSoundController>().PlayLevel2Sound(0);
            await a7Seconds;//01Thank you for helping, my friend! You have been a great librarian! We will make a good team together.
            await a3Seconds;

            Instantiate(Resources.Load("Success Canvas"), transform.position, Quaternion.identity, transform);
        }
        else
        {
            //books.GetComponent<Books>().MakeBooksUnInteractable();
            await new WaitForSeconds(2);
            Instantiate(Resources.Load("Un Success Canvas"), transform.position, Quaternion.identity, transform);
        }


    }

    public void SkipIntroLevel()
    {
        //close the speaker AudioSource

        if(battery==null) battery = batteryObject.AddComponent<Battery>();
        if(bird==null) bird = (GameObject)Instantiate(Resources.Load("Bird"), transform.position, Quaternion.identity);
        introLevelSkipped = true;
        bird.GetComponentInChildren<AudioSource>().enabled = true;
        GameManager.instance.FireOnLevelBegin();
        speaker.StopSound();
        StopAllCoroutines();
    }


    public void ControlBatteryBarsAndRobotStateDecode()
    {
        if (!Statistics.instance.android) return;
        if (Statistics.instance.correctPutBooksNo == 30) battery.ControlBatteryBarsAndRobotState(6); 
        else if (Statistics.instance.correctPutBooksNo >= 28) battery.ControlBatteryBarsAndRobotState(5);
        else if (Statistics.instance.correctPutBooksNo >= 26) battery.ControlBatteryBarsAndRobotState(4);
        else if (Statistics.instance.correctPutBooksNo >= 24) battery.ControlBatteryBarsAndRobotState(3);
        else if (Statistics.instance.correctPutBooksNo >= 22) battery.ControlBatteryBarsAndRobotState(2);
        else if (Statistics.instance.correctPutBooksNo >= 20) battery.ControlBatteryBarsAndRobotState(1);
        else battery.ControlBatteryBarsAndRobotState(0);
    }

}

//deprecated code 
//int yellowBooksNo = 0;
//int blueBooksNo = 0;
//int redBooksNo = 0;
//public void ControlBatteryBarsAndRobotState(string bookTag, bool increase)
//{
//    if (bookTag == "BookYellow")
//    {
//        if (increase) yellowBooksNo++;
//        else yellowBooksNo--;
//        battery.ControlBars(1, ReturnState(yellowBooksNo));
//    }
//    else if (bookTag == "BookBlue")
//    {
//        if (increase) blueBooksNo++;
//        else blueBooksNo--;
//        battery.ControlBars(0, ReturnState(blueBooksNo));
//    }
//    else
//    {
//        if (increase) redBooksNo++;
//        else redBooksNo--;
//        battery.ControlBars(2, ReturnState(redBooksNo));
//    }
//    if (Statistics.instance.correctPutBooksNo == 26) robot.OpenLeg();
//    else if (Statistics.instance.correctPutBooksNo == 22) robot.OpenEye();
//}

//int ReturnState(int number)
//{
//    if (number > 8) return 2;
//    else if (number == 8) return 1;
//    else return 0;
//}

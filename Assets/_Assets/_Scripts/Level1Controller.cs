using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1Controller : MonoBehaviour,ILevel
{

    GameObject books;
    Robot robot ;
    GameObject level1Introanimations;
    EffectsController effectsController;
    GameObject battery;
    GameObject firstPutBook;//this book must be invisible till the robot put it 
    bool introLevelSkipped = false;
    NPCSoundController speaker;
    #region wait times
    WaitForSeconds a3Seconds;
    WaitForSeconds a5Seconds;
    #endregion
    private void Start()
    {
        books =FindObjectOfType<Books>().gameObject;
        robot = FindObjectOfType<Robot>();
        level1Introanimations =GameObject.Find("Level1Introanimations");
        battery =GameObject.Find("Battery");
        speaker = FindObjectOfType<NPCSoundController>();
        effectsController = FindObjectOfType<EffectsController>();

        #region wait times
        a3Seconds = new WaitForSeconds(3f);
        a5Seconds = new WaitForSeconds(5f);
        #endregion
    }

    public void BeginLevel()
    {
        //Debug.Log("level1 controller.BeginLevel()");
        if (!introLevelSkipped) 
            StartCoroutine(BeginLevel1());
        //await BeginLevel1();
    }
    IEnumerator BeginLevel1()
    {
        //Debug.Log("introLevelSkipped state after begin level :"+ introLevelSkipped);
        yield return a3Seconds;
        firstPutBook =GameObject.Find("Collidier29").transform.GetChild(8).transform.gameObject;
        //disappear book parts
        for (int i = 0; i < 4; i++)
        {
            firstPutBook.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = false;
        }
        //all the code when art be ready
        speaker.PlayLevel1Sound(0);
        yield return new WaitForSeconds(7);//00Hello Im Adam, Im the one who is talking from the speaker .. Look over here
        speaker.PlayLevel1Sound(1);
        yield return new WaitForSeconds(2);//01you want to know what happened here,

        robot.SetLevelIntroPosition(GameObject.Find("RobotIntialPosition").transform);
        robot.Open();
        //when flash back
        books.SetActive(false);
        effectsController.StartLevel1IntroEffect();
        yield return a3Seconds;// There was a robot named leo.
        speaker.PlayLevel1Sound(2);//  
        level1Introanimations.GetComponent<Animator>().enabled = true;
        robot.Walk();
        yield return a3Seconds;//3 02 There was a robot named leo.
        //speaker.PlayLevel1Sound(2);// 03One day while he was walking around and doing his work, the electricity went off and he ran out of battery.Then, He fell off under the books
        robot.Idle();
        yield return a3Seconds;//6 He was the librarian here
        robot.Walk();
        yield return new WaitForSeconds(6f);//15 and doing his work,
        speaker.PlayLevel1Sound(3);// 03One day while he was walking around and doing his work, the electricity went off and he ran out of battery.Then, He fell off under the books
        yield return a3Seconds;//One day while he was walking around
        robot.Idle();
        yield return a3Seconds;//18 //and doing his work, the electricity 
        effectsController.StopLevel1IntroEffect();
        effectsController.StartFadeToDark();
        robot.SetAtHome();
        yield return a3Seconds; //went off and he ran out of battery.Then, He fell off under the books
        effectsController.StopFadeToDark();
        level1Introanimations.SetActive(false);
        books.SetActive(true);
        //appear book parts
        for (int i = 0; i < 4; i++)
        {
            firstPutBook.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = true;
        }
        yield return new WaitForSeconds(4);
        speaker.PlayLevel1Sound(4);
        yield return a3Seconds;//04Can you help in getting him out
        speaker.PlayLevel1Sound(5);
        yield return new WaitForSeconds(4);//05Returning the books to the shelf will help
        speaker.PlayLevel1Sound(6);
        yield return a3Seconds;//06lets get started!!!

        //EndLevel();
        GameManager.instance.FireOnLevelBegin();
    }

    public void EndLevel()
    {
        StartCoroutine(EndLevel1());
    }

    IEnumerator EndLevel1()
    {
        books.GetComponent<Books>();
        if (GameManager.instance.successed)
        {
            robot.SetAtHome();
            battery.GetComponent<Animator>().enabled = true;
            battery.GetComponent<Animator>().SetBool("RiseUp", true);
            battery.GetComponent<AudioSource>().Play();
            yield return a3Seconds;
            speaker.PlayLevel1Sound(7);
            yield return new WaitForSeconds(9);
            Instantiate(Resources.Load("Success Canvas"), transform.position, Quaternion.identity, transform);
        }
        else
        {
            //books.GetComponent<Books>().MakeBooksUnInteractable();
            yield return new WaitForSeconds(2);
            Instantiate(Resources.Load("Un Success Canvas"), transform.position, Quaternion.identity, transform);
        }
    }


    public void SkipIntroLevel()
    {
        Debug.Log("SkipIntroLevelBegining");
        introLevelSkipped = true;
        if (firstPutBook != null)
        {
            for (int i = 0; i < 4; i++)
            {
                firstPutBook.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = true;
            }
        }
        //Debug.Log("SkipIntroLevel after for");
        effectsController.StopLevel1IntroEffect();
        effectsController.StopFadeToDark();
        //Debug.Log("SkipIntroLevel after effectsController");
        robot.Idle();
        robot.SetAtHome();
        level1Introanimations.SetActive(false);
        //Debug.Log("SkipIntroLevel after level1Introanimations");
        books.SetActive(true);
        Laser laser = FindObjectOfType<Laser>();
        if (laser != null) laser.gameObject.SetActive(false);
        GameManager.instance.FireOnLevelBegin();
        speaker.StopSound();
        StopAllCoroutines();
        //Debug.Log("SkipIntroLevelEnding");
    }

}

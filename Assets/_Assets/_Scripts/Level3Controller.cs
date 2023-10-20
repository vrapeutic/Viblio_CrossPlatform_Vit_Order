using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Level3Controller : MonoBehaviour, ILevel
{
    Books books;
    Robot robot;
    RobotSoundController robotSound;
    GameObject level3Introanimations;
    GameObject level3RobotAnimations;
    bool introLevelSkipped = false;
    HandHider rightHand;
    HandHider leftHand;
    #region wait times
    WaitForSeconds a3Seconds;
    WaitForSeconds a5Seconds;
    #endregion

    private void Start()
    {
        books = FindObjectOfType<Books>();
        level3Introanimations = GameObject.Find("Level3IntroAnimations");
        robot = FindObjectOfType<Robot>();
        robotSound =FindObjectOfType<RobotSoundController>();
        level3RobotAnimations = GameObject.Find("RobotAnimations");
        rightHand = GameObject.FindGameObjectWithTag("RightHand").GetComponent<HandHider>();
        leftHand = GameObject.FindGameObjectWithTag("LeftHand").GetComponent<HandHider>();
        #region wait times
        a3Seconds = new WaitForSeconds(3);
        a5Seconds = new WaitForSeconds(5);
        #endregion
    }

    public void BeginLevel()
    {
        //if (!introLevelSkipped) StartCoroutine(BeginLevel3());
    }

    IEnumerator BeginLevel3()
    {
        if (introLevelSkipped) yield break;

        robot.Idle();
        robot.SetLevelIntroPosition(GameObject.Find("RobotIntialPosition").transform);
        yield return a3Seconds;
        robotSound.PlayLevel3Sound(0);        // Hello my friend! You have been working so hard to help me out. 
        yield return a5Seconds;
        level3Introanimations.GetComponent<Animator>().enabled = true;
        robotSound.PlayLevel3Sound(1);
        robot.Walk();
        yield return a3Seconds;//3s//03Now that I am all charged up.,+                             
        robotSound.PlayLevel3Sound(2);
        robot.Idle();
        yield return new WaitForSeconds(4);//7s//we will be working together to organize these books
        robotSound.PlayLevel3Sound(3);
        //These books are organized in a sequenced pattern
        yield return new WaitForSeconds(4);//11s//05You will be responsible for returning books to this bookshelf.
        robotSound.PlayLevel3Sound(4);
        // watch the sequence carefully
        yield return a5Seconds;//16s//06These books are organized in a sequential pattern,
        robotSound.PlayLevel3Sound(5);
        yield return new WaitForSeconds(2); //18//07watch the sequence carefully. and complete the missing books with the correct color!
        FindObjectOfType<HighLightBooks>().MakeHighLightBooks();
        yield return new WaitForSeconds(7);//25
        robot.Walk();
        yield return a3Seconds;//28
        robot.Idle();
        GameManager.instance.FireOnLevelBegin();
    }

    

    public async void EndLevel()
    {
        if (GameManager.instance.successed)
        {
            robot.transform.SetParent(null);
            robot.Idle();
            await new WaitForSeconds(3);
            robotSound.PlayLevel3Sound(8);
            await new WaitForSeconds(7);//10Wow, you did it!! Thanks for helping, my friend.
            Instantiate(Resources.Load("Success Canvas"), transform.position, Quaternion.identity, transform);
        }
        else
        {
            //books.MakeBooksUnInteractable();
            await new WaitForSeconds(2);
            Instantiate(Resources.Load("Un Success Canvas"), transform.position, Quaternion.identity, transform);
        }

    }

    public void SkipIntroLevel()
    {
        //Debug.Log("SkipIntroLevel");
        introLevelSkipped = true;
        robot.transform.SetParent(null);
        //Debug.Log("SkipIntroLevelEnd");
        if (FindObjectOfType<Laser>() != null) FindObjectOfType<Laser>().Deactive();
        level3Introanimations.SetActive(false);
        //GameObject.FindGameObjectWithTag("Laser").SetActive(false);
        //if (laser != null) laser.gameObject.SetActive(false);
        //Debug.Log(laser.gameObject.name);
        robot.Idle();
        GameManager.instance.FireOnLevelBegin();
        robotSound.StopSound();
        StopAllCoroutines();
    }
}

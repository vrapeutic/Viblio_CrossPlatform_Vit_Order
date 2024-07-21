using System.Collections;
using System.Threading.Tasks;
using Tachyon;
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
        InvokationManager invokationManager = new InvokationManager(this, this.gameObject.name);
        NetworkManager.InvokeClientMethod("CompleteDistractingTaskRPC", invokationManager);
        books = FindObjectOfType<Books>();
        level3Introanimations = GameObject.Find("Level3IntroAnimations");
        robot = FindObjectOfType<Robot>();
        robotSound =FindObjectOfType<RobotSoundController>();
        level3RobotAnimations = GameObject.Find("Level3RobotAnimations");
        rightHand = GameObject.FindGameObjectWithTag("RightHand").GetComponent<HandHider>();
        leftHand = GameObject.FindGameObjectWithTag("LeftHand").GetComponent<HandHider>();
        #region wait times
        a3Seconds = new WaitForSeconds(3);
        a5Seconds = new WaitForSeconds(5);
        #endregion
    }

    public void BeginLevel()
    {
        if (!introLevelSkipped)
            StartCoroutine(BeginLevel3());
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
        BeginRobotDistracting();//BeginLevel3();
        GameManager.instance.FireOnLevelBegin();
    }

    async void BeginRobotDistracting()
    {
        robot.SetLevelIntroPosition(level3RobotAnimations.transform.GetChild(0));
        robot.Idle();
        robot.Walk();
        level3RobotAnimations.GetComponent<Animator>().enabled = true;
        await new WaitForSeconds(30);
        if (!GameManager.instance.currentlyPlaying) return;
        level3RobotAnimations.GetComponent<Animator>().enabled = false;
        robot.Idle();
        robot.DropDown();
        books.MakeBooksUnInteractableWhileSelectingThem();
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
        await a5Seconds;
        robotSound.PlayLevel3Sound(7);//09Can you please touch my broken leg in the right place
    }

    public void CompleteDistractingTask()
    {
        NetworkManager.InvokeServerMethod("CompleteDistractingTaskRPC", this.gameObject.name);
    }
    public async void CompleteDistractingTaskRPC()
    {
        await CompleteDistractingTaskIenum();
        robot.Walk();
        level3RobotAnimations.GetComponent<Animator>().enabled = true;
        books.MakeBooksInteractable();
        BeginRobotDistracting();
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

    IEnumerator CompleteDistractingTaskIenum()
    {
        robot.RiseUp();
        //robotSound.PlayLevel3Sound(9);
        yield return a3Seconds;

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


    private void OnEnable()
    {
        //GameManager.instance.FireOnCompleteDistractingTask 
        GameManager.OnCompleteDistractingTask += CompleteDistractingTask;
    }

    private void OnDisable()
    {
        GameManager.OnCompleteDistractingTask -= CompleteDistractingTask;
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
        BeginRobotDistracting();
        GameManager.instance.FireOnLevelBegin();
        robotSound.StopSound();
        StopAllCoroutines();
    }
}

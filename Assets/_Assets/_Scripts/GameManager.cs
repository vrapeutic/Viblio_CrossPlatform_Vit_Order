using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static event Action OnLevelBegin;
    public static event Action OnLevelEnd;
    public bool currentlyPlaying = false;//While the level is currently playing or not
    public bool successed = false;
    AudioListener[] listeners;
    GameObject levelManager;
    Level2Controller level2Controller;
    [SerializeField] BoolValue isTestingMode;
    [SerializeField] BoolValue isStandalone;
    [SerializeField] GameEvent OnMenuAppear;
    [SerializeField] IntVariable noOfBooks; 
    WaitForSeconds a1AndHalfSecond;
    Books books;

    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            //Debug.Log("the else!");
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        //StatisticsJsonFile.Instance.data.attempt_start_time = System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss tt");
        levelManager = FindObjectOfType<LevelManager>().gameObject;
        books = FindObjectOfType<Books>();
        if (levelManager==null) Debug.Log("levelManager is null");
        if (Statistics.instance.level == 2) level2Controller = FindObjectOfType<Level2Controller>();
        MuteSounds();
        if (!Statistics.instance.android) return;
        //Debug.Log("GameManager.Start()");
        StartCoroutine(BeginLevelIenum());
        a1AndHalfSecond = new WaitForSeconds(1.5f);
        Statistics.instance.OnLevelBegin();
        books.MakeBooksUnInteractable();

    }

    private void OnEnable()
    {
 /**/       //OVRGrabber.OnGrabBegin += StopResponseTimer;
        //OVRGrabber.OnGrabEndWithGrabbable += CheckCorrectPutBookNumbers;
    }

    private void OnDisable()
    {
        //OVRGrabber.OnGrabBegin -= StopResponseTimer;
        //OVRGrabber.OnGrabEndWithGrabbable -= CheckCorrectPutBookNumbers;
    }


    public void StopResponseTimer()
    {
        Statistics.instance.StopResponseTimer();
    }


    IEnumerator BeginLevelIenum()
    {
        //Debug.Log("GameManager.BeginLevelIenum()");
        yield return new WaitForSeconds(2);
        UnMutesounds();
        //Debug.Log("GameManager.BeginLevelRPC()");
        //try
        //{
        //    levelManager.GetComponent<ILevel>().BeginLevel();
        //}
        //catch
        //{
        //    Debug.Log("levelManager.GetComponent<ILevel>() is: " + levelManager.GetComponent<ILevel>());
        //}
        FireOnLevelBegin();
    }

    #region control attempt variables
    public void CheckCorrectPutBookNumbers()
    {
        StartCoroutine(CheckCorrectPutBookNumbersInum());
    }
    IEnumerator CheckCorrectPutBookNumbersInum()
    {
        yield return a1AndHalfSecond;
        if (Statistics.instance.level == 2) level2Controller.ControlBatteryBarsAndRobotStateDecode();
        Debug.Log("**"+Statistics.instance.correctPutBooksNo+"**"+ noOfBooks.Value);

        if (Statistics.instance.correctPutBooksNo >= noOfBooks.Value)//end the experience
        {
            EndSuccessfully();
        }
    }
    #endregion

    #region fire events
    public void FireOnLevelBegin()
    {
        //Debug.Log("FireOnLevelBegin");
        currentlyPlaying = true;
        if (!Statistics.instance.android) return;
        OnLevelBegin();
        Statistics.instance.StartSessionTimer();
        books.MakeBooksInteractable();
    }

    public void FireOnLevelEnd()
    {
        OnLevelEnd();
        Statistics.instance.StopResponseTimer();
        books.MakeBooksUnInteractable();
        //Debug.Log("FireOnLevelEnd");
    }
    #endregion

    #region End Cases
    public void EndSuccessfully()//Ends when put all books
    {
        if (!currentlyPlaying) return;
        currentlyPlaying = false;
        //Debug.Log("EndSuccessfully");
        successed = true;
        StartCoroutine(End());
    }

    public void EndUnSuccessfully()//Ends when time out 
    {
        Debug.Log("    public void EndUnSuccessfully()");
        if ( !currentlyPlaying) return;
        Debug.Log("        if ( !currentlyPlaying) return;");
        successed = false;
        StartCoroutine(End());
        //Instantiate(Resources.Load("Success Canvas"), transform.position, Quaternion.identity, transform);
    }

    IEnumerator End()
    {
        currentlyPlaying = false;
        if (Statistics.instance.android)
        {
            Debug.Log("I Will send attempt statistics");
            Statistics.instance.SendAttemptStatistics();
            Debug.Log("*before SessionController.instance.canLoadSessionCanvas=true;");
            try
            {
                SessionController.instance.canLoadSessionCanvas=true;
            }
            catch (Exception)
            {
                Debug.Log("can`t find session controller");
            }
            Debug.Log("*before FireOnLevelEnd();");
            FireOnLevelEnd();
        }
        //levelManager.GetComponent<ILevel>().EndLevel();

        if (successed)
        {
            yield return new WaitForSeconds(1);

                OnMenuAppear.Raise();
                if (Statistics.instance.languageIndex == 0) Instantiate(Resources.Load("End Successfully Canvas Standalone"), transform.position, Quaternion.identity, transform).name = "End Successfully Screen Standalone";
                else Instantiate(Resources.Load("End Successfully Canvas Standalone VIT"), transform.position, Quaternion.identity, transform).name = "End Successfully Screen Standalone";
            yield return new WaitForSeconds(120);
            SceneManager.LoadSceneAsync("Main");
        }
        else
        {
            yield return new WaitForSeconds(3);

                OnMenuAppear.Raise();
                if (Statistics.instance.languageIndex == 0) Instantiate(Resources.Load("End UnSuccessfully Canvas Standalone"), transform.position, Quaternion.identity, transform).name = "End Successfully Screen Standalone";
                else Instantiate(Resources.Load("End UnSuccessfully Canvas Standalone VIT"), transform.position, Quaternion.identity, transform).name = "End Successfully Screen Standalone";
            yield return new WaitForSeconds(120);
            SceneManager.LoadSceneAsync("Main");
        }

    }
    #endregion

    #region control sound
    void MuteSounds()
    {
        AudioListener.volume = 0;
    }

    async void UnMutesounds()
    {
        await new WaitForSeconds(3f);
        AudioListener.volume = 1;
    }
    #endregion

}

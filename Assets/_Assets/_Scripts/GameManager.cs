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
    [SerializeField] GameEvent OnSeccessAtempt;
    [SerializeField] GameEvent OnEndSuccessfully;
    [SerializeField] GameEvent OnEndUnSuccessfully;
    [SerializeField] IntVariable noOfBooks; 
    WaitForSeconds a1AndHalfSecond;
    Books books;
    int lastCorrectAttempts = 0;
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

    public void StopResponseTimer()
    {
        Statistics.instance.StopResponseTimer();
    }


    IEnumerator BeginLevelIenum()
    {
        //Debug.Log("GameManager.BeginLevelIenum()");
        yield return new WaitForSeconds(2);
        UnMutesounds();
        //    levelManager.GetComponent<ILevel>().BeginLevel();
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
        //if (Statistics.instance.level == 2) level2Controller.ControlBatteryBarsAndRobotStateDecode();
        Debug.Log("**"+Statistics.instance.correctPutBooksNo+"**"+ noOfBooks.Value);
        if(Statistics.instance.correctPutBooksNo > lastCorrectAttempts)
        {
            OnSeccessAtempt.Raise();
            lastCorrectAttempts = Statistics.instance.correctPutBooksNo;
        }
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
        OnEndSuccessfully.Raise();
        currentlyPlaying = false;
        //Debug.Log("EndSuccessfully");
        successed = true;
        StartCoroutine(End());
    }

    public void EndUnSuccessfully()//Ends when time out 
    {
        if ( !currentlyPlaying) return;
        successed = false;
        OnEndUnSuccessfully.Raise();
        StartCoroutine(End());
        //Instantiate(Resources.Load("Success Canvas"), transform.position, Quaternion.identity, transform);
    }

    IEnumerator End()
    {
        currentlyPlaying = false;
            FireOnLevelEnd();
        //levelManager.GetComponent<ILevel>().EndLevel();

        if (successed)
        {
            yield return new WaitForSeconds(1);

                OnMenuAppear.Raise();
            Debug.Log("$$game Ended ="+Statistics.instance.languageIndex);
            if (Statistics.instance.languageIndex == 0) Instantiate(Resources.Load("End Successfully Canvas Standalone"), transform.position, Quaternion.identity, transform).name = "End Successfully Screen Standalone";
            else Instantiate(Resources.Load("End Successfully Canvas Standalone VIT"), transform.position, Quaternion.identity, transform).name = "End Successfully Screen Standalone";
        }
        else
        {
            yield return new WaitForSeconds(1);

                OnMenuAppear.Raise();
                if (Statistics.instance.languageIndex == 0) Instantiate(Resources.Load("End UnSuccessfully Canvas Standalone"), transform.position, Quaternion.identity, transform).name = "End Successfully Screen Standalone";
                else Instantiate(Resources.Load("End UnSuccessfully Canvas Standalone VIT"), transform.position, Quaternion.identity, transform).name = "End Successfully Screen Standalone";
        }
        yield return new WaitForSeconds(5);
        if(SceneManager.GetSceneByBuildIndex(0).name=="SystemLobby") Application.Quit();

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

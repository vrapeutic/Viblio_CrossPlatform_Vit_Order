using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;
using System;
using OldStats;

public class Statistics : MonoBehaviour
{
    public static Statistics instance;

    #region Selected Data
    public int level=2;
    public int totalBooks=30;
    public int languageIndex = 0;//0 for english, 1 for vit
    //[HideInInspector]
    public bool isClosedTime = false;
    //[HideInInspector]
    public int closedTimeValue = 60;
    public bool isWithRay = false;
    float typicalTime;
    [SerializeField]
    float level1TypicalTime = 60.0f;
    [SerializeField]
    float level2TypicalTime = 90f;
    [SerializeField]
    float level3TypicalTime = 60.0f;


    float tas;
    [SerializeField]
    float level1tas = 55.0f;
    [SerializeField]
    float level2tas = 80.0f;
    [SerializeField]
    float level3tas = 110.0f;
    #endregion

    #region Gathered Data
    public int correctPutBooksNo=0;
    public int wrongTrials;
    public int totalTrials=0;
    [HideInInspector]
    public float actualTime = 0;
    float responseTimer=0;
    float sessionTimer = 0;
    List<float> responseTimers = new List<float>();
    #endregion

    bool canCountResponeTimer=false;
    bool canCountSessionTimer = false;
    float aas;
    private int distractiblityScore;
    public bool android=false;
    //CSVFirebaseFileWriter cSVFirebaseFileWriter;
    #region Unity Call backs
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
        #if UNITY_ANDROID
        android = true;
        #endif
    }

    private void Start()
    {
       // cSVFirebaseFileWriter = new CSVFirebaseFileWriter();
    }

    #endregion

    public void SendAttemptStatistics()
    {

        SessionStats.Instance.data.impulsivity_score = CalculateImpulsivityScore();
        SessionStats.Instance.data.response_time = Convert.ToSingle(responseTimers.Count > 0 ? responseTimers.Average() : 0.0);
        SessionStats.Instance.data.omission_score = CalculateOmisionScore();
        SessionStats.Instance.data.distraction_endurance_score = SessionStats.Instance.data.distractibility_score = CalculateDistractiblityScore();
        SessionStats.Instance.data.actual_duration_in_seconds = sessionTimer;
        SessionStats.Instance.data.attempt_end_time = System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss tt");
        SessionStats.Instance.data.correct_attempts = correctPutBooksNo;
        SessionStats.Instance.data.wrong_attempts = wrongTrials;
        if (Books.instatnce.bookshights.Count > 0)
        {
            SessionStats.Instance.data.minimum_book_put_height = Books.instatnce.bookshights.Min() * 100;
            SessionStats.Instance.data.maximum_book_put_height = Books.instatnce.bookshights.Max() * 100;
            SessionStats.Instance.data.average_book_put_height = Books.instatnce.bookshights.Average() * 100;
        }
        BackendSession.instance.SendStatsData();
        //cSVFirebaseFileWriter.SaveScvfile();
        StopSessionTimer();
        //Debug.Log(JsonUtility.ToJson(StatisticsJsonFile.Instance));
        //if (JsonAPIS.instance != null)
        //{
        //    JsonAPIS.instance.PostRequest(StatisticsJsonFile.Instance);
        //    Debug.Log("ServerRequest.instance != null");
        //}
        //else Debug.Log("ServerRequest.instance == null");
    }


    #region  Score Calculations
    float CalculateImpulsivityScore()
    {
        actualTime = sessionTimer;
        float tir =actualTime/typicalTime;
        if (isClosedTime) tir = actualTime/closedTimeValue ;
        float tar = CalculateTar();
        float impulsivityScore= (float)(1 / ((-tar) * ((Mathf.Log10(tir) - 1) + Mathf.Epsilon)));
        if (isClosedTime) Debug.Log("tir: " + tir + "time taken: " + actualTime + " ,closed time value: " + closedTimeValue);
        else Debug.Log("tir: " + tir + "time taken: " + actualTime + " ,typical time: " + typicalTime); ;
        return impulsivityScore;
    }
    //tar is targets ratio
    float CalculateTar()
    {
        int totalTrials = correctPutBooksNo + wrongTrials;
        float tarTirm1 = ((float)(correctPutBooksNo) / totalBooks);
        float tartirm2 = ((float)(correctPutBooksNo) / totalTrials);
        float tar = (tarTirm1 - (1 - tartirm2) + 1) / 2;
        if (totalTrials == 0) tar = 0;
        //Debug.Log("Tar: " + tar + ", correct put book: " + correctPutBooksNo+", Total no of available Books: "+totalBooks+", total trials: "+totalTrials);
        return tar;
    }

    float CalculateOmisionScore()
    {
        aas = FindObjectOfType<Attention>().AttentionTimer;//actual attention span
        SessionStats.Instance.data.actual_attention_time = aas;
        float omisionScore = (tas / (aas + Mathf.Epsilon));
        Debug.Log("omisionScore: " + omisionScore+"aas: " + aas + ",tas: " + tas );
        return omisionScore;

    }

    float CalculateDistractiblityScore()
    {
        float tfd = sessionTimer - aas;//time folllowing distractor
        float distractibilityScore = 1 - (tfd / tas);
        if (level == 1) distractibilityScore = 0;
        Debug.Log("distractibilityScore: " + distractibilityScore+",tfd: " + tfd );
        return distractibilityScore;   
    }
    #endregion

    #region Response Timer
    public async void StartResponseTimer()
    {
        canCountResponeTimer = true;
        responseTimer = 0;
        await StartResponseTimerIenum();
    }

    IEnumerator StartResponseTimerIenum()
    {
        while (canCountResponeTimer)
        {
            responseTimer += Time.deltaTime;
            yield return null;
        }
        //Debug.Log("Response timer : " + responseTimer);
    }

    public void StopResponseTimer()
    {
        canCountResponeTimer = false;
        if(responseTimer!=0) responseTimers.Add(responseTimer);
        responseTimer = 0;
    }
    #endregion

    #region Session Timer
    public async void StartSessionTimer()
    {
        if (!android) return;
        Debug.Log("StartSessionTimer");
        canCountSessionTimer = true;
        sessionTimer = 0;
        await StartSessionTimerIenum();
    }

    IEnumerator StartSessionTimerIenum()
    {
        //Debug.Log("StartSessionTimerIenum "+ "isClosedTime "+ isClosedTime+ " canCountSessionTimer "+ canCountSessionTimer);
        while (canCountSessionTimer)
        {
            sessionTimer += Time.deltaTime;
            //Debug.Log("* current session timer :"+sessionTimer +isClosedTime);
            if (isClosedTime)
            {
                if (sessionTimer >= closedTimeValue)
                {
                    //Debug.Log("#sessionTimer >= closedTimeValue");
                    GameManager.instance.EndUnSuccessfully();
                    yield break;
                }
            }
            yield return null;
        }
    }

    public void StopSessionTimer()
    {
        Debug.Log("#stop session timer called");
        canCountSessionTimer = false;
    }
    #endregion

    #region Intialization

    //intiat some default values when Main menu loaded
    public void OnStart()
    {
        level = 1;
        totalBooks = 10;
        isClosedTime = false;
        closedTimeValue = 60;
        wrongTrials = 0;
        actualTime = 0;
        totalTrials = 0;
        sessionTimer = 0;
        isWithRay = false;
    }

    //iniat some variables when the actual level begun
    public void  OnLevelBegin()
    {
        correctPutBooksNo = 0;
        wrongTrials = 0;
        SessionStats.Instance.data.attempt_start_time = System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss tt");
        SessionStats.Instance.data.level = Statistics.instance.level.ToString();
        sessionTimer = 0;
        if (level == 1)
        {
            totalBooks = 10;
            typicalTime = level1TypicalTime;
            tas = level1tas;
        }
        else if (level == 2)
        {
            totalBooks = 30;
            typicalTime = level2TypicalTime;
            tas = level2tas;
        }
        else
        {
            totalBooks = 30;
            typicalTime = level3TypicalTime;
            tas = level3tas;
        }
        if (isClosedTime)
        {
            SessionStats.Instance.data.attempt_type = "closed time";
            SessionStats.Instance.data.expected_duration_in_seconds = closedTimeValue;
        }
        else
        {
            SessionStats.Instance.data.attempt_type = "Opened time";
            SessionStats.Instance.data.expected_duration_in_seconds = typicalTime;
        }
        if (BackendSession.instance.currentRoomId == null) BackendSession.instance.StartSession();


    }
   
    #endregion
}

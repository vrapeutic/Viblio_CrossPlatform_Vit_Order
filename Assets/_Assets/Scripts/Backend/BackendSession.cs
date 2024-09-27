using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackendSession : MonoBehaviour
{
    public static BackendSession instance;
    BackendAPIS jsonAPIS = new BackendAPIS();
    SessionStats CurrentStats;
    CreateSession sessionElements= new CreateSession() ;
    [SerializeField]GameEvent OnSeccessPatientIdLogin;
    [SerializeField]GameEvent OnFaildPatientIdLogin;
    public DataCollection MyStats;
    int sessionId;
    public int moduleID = 13;//every module has unique id
    public  string patient_id;
    public string auth="";
    [HideInInspector]
    public  string currentRoomId;
    [HideInInspector]
    public string startTimeSession = "yyyy/MM/dd hh:mm:ss tt"; // use System.DateTime.Now(start time) when level start to getstart time
    [HideInInspector]
    public string endTimeNow = "yyyy/MM/dd hh:mm:ss tt"; //use System.DateTime.Now(end time) when level end to get end time

    private void Awake()
    {

        //instance = FindObjectsOfType<BackendSession>();
        //if (instance.Length > 1)
        //    Destroy(instance[1].gameObject);
        //DontDestroyOnLoad(this.gameObject);
        if (BackendSession.instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);

    }

    public void SetPatient(string _patientId)
    {
        patient_id = _patientId;
        StartSession();
    }

    public string GetPatient()
    {
        return patient_id;
    }
  
    #region SessionStart
    public void StartSession()
    {
        StartCoroutine(SendSessionElements(patient_id, moduleID,auth));
    }
   
    IEnumerator SendSessionElements(string patient_id , int moduleId,string auth)
    {
        yield return StartCoroutine(jsonAPIS.SendSessionElements(patient_id, moduleID,auth));
        sessionElements = jsonAPIS.SessioResponseElements();

        if (sessionElements.room_id == "")
        {
            OnFaildPatientIdLogin.Raise();
        }
        else
        {
            Debug.Log("sessionElements.room_id: " + sessionElements.room_id);
            OnSeccessPatientIdLogin.Raise();
            StartGame(sessionElements.id, sessionElements.room_id);
        }
    }

    public void StartGame(int getId, string getRoom)
    {
        sessionId = getId;
        currentRoomId = getRoom;
        if (currentRoomId != null)
        {
            startTimeSession = System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss tt");
        }
    }
    #endregion
    #region SendSessionData
    public void SendStatsData()
    {
        //jsonAPIS.SetStats(MyStats);

        StartCoroutine(SendStatsIEnum());
    }
    public IEnumerator SendStatsIEnum()
    {
        yield return StartCoroutine(jsonAPIS.SendSessionData( auth));
        CurrentStats = jsonAPIS.SendStatsResponse();
        currentRoomId = null;
    }
    #endregion
    #region GetStatsData
    /*  public void GetSTatsData()
      {
          StartCoroutine(GetStats(moduleID, patient_id));
      }
      IEnumerator GetStats(int VR_id, int patient_id)
      {
          Debug.Log("exit");
          if (save == false)
          {
              yield return StartCoroutine(jsonAPIS.GetSessionData(VR_id, patient_id));
              statsData = jsonAPIS.SessionDataResponse();
              save = true;
              Invoke("Exit",3f);
              Debug.Log("put");
          }
      }

      private void OnApplicationQuit()
      {
          if (quit == false)
          {
              OnExit();
              Application.CancelQuit();
          }
          if (quit == true)
              Application.Quit();
      }


      public void OnExit()
      {
          SendPutRequest();

      }*/
    #endregion
    #region Exit
    public void Exit()
    {
        Debug.Log("quit");
        Application.Quit();

    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendStatsExample : MonoBehaviour
{
    BackendSession currentSession;
    DataCollection dataCollection;
    private void Awake()
    {
        currentSession = FindObjectOfType<BackendSession>();
        dataCollection = FindObjectOfType<BackendSession>().MyStats;
    }
    public void UpdateData()
    {
        dataCollection.level = "2";
        dataCollection.omission_score = 2;
        dataCollection.attempt_type = "Closed"; // you can edit all data with same method
    }

   public void SendUpdatedData()
   {
        currentSession.SendStatsData();   
    }
}

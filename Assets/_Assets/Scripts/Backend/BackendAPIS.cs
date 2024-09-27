using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BackendAPIS : MonoBehaviour
{
    SessionElements getSessionData;
    DataCollection currentData;
    SessionStats CurrentStats;
    CreateSession sessionElements =new CreateSession();

    #region CreateSession
    public IEnumerator SendSessionElements(string patient_id,int vr_module_id, string auth)
    {
        string json = ConvertElementsTojson(patient_id, vr_module_id);
        byte[] postData = System.Text.Encoding.UTF8.GetBytes(json);
        string url = "http://vrapeutic-rails-production.eba-2nd4efne.eu-west-1.elasticbeanstalk.com/api/v1/module_sessions/create_without_headset/";//"https://dashboard.myvrapeutic.com/api/v1/module_sessions/create_without_headset/";
        UnityWebRequest www = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        Debug.Log("StartSession :" + json);
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("Authorization", "Bearer "+auth);
        yield return www.SendWebRequest();
        Debug.Log("www.url"+www);
        if (www.error != null)
        {
            Debug.Log("error: " + www.error);
        }
        else if (www.downloadHandler.text != null)
        {
            sessionElements = JsonUtility.FromJson<CreateSession>(www.downloadHandler.text);
            Debug.Log("all" + www.downloadHandler.text);
        }
    }
    string ConvertElementsTojson(string patient_id, int vr_module_id)
    {
        SessionElements elements = new SessionElements();
        elements.patient_id = patient_id;
        elements.vr_module_id = vr_module_id;
        return JsonUtility.ToJson(elements);
    }
    public CreateSession SessioResponseElements()
    {
        return sessionElements;
    }
    #endregion  
    #region setStats
    public DataCollection SetStats(DataCollection _currentData)
    {
        return currentData = _currentData;
    }
    #endregion
    #region SendStats
   public IEnumerator SendSessionData(string auth)
    {
        string json = ConvertDataToJson( );
        byte[] postData = System.Text.Encoding.UTF8.GetBytes(json);
        string url = "http://vrapeutic-rails-production.eba-2nd4efne.eu-west-1.elasticbeanstalk.com/api/v1/statistics/create_without_headset";//"https://dashboard.myvrapeutic.com/api/v1/statistics/create_without_headset";
        UnityWebRequest www = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        Debug.Log("SendjsonIEnum :" + json);
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("Authorization", "Bearer " + auth);
        yield return www.SendWebRequest();
        CurrentStats = JsonUtility.FromJson<SessionStats>(www.downloadHandler.text);
        Debug.Log("Send request Resonce code: " + www.responseCode);
        if (www.error != null)
        {
            Debug.Log("error" + www.error);
        }
        else if (www.downloadHandler.text != null)
        {

            Debug.Log("all" + www.downloadHandler.text);
        }

    }
    string ConvertDataToJson()

    {
        SessionStats.Instance.room_id = sessionElements.room_id;
        SessionStats.Instance.data.session_start_time = sessionElements.session_date;

        return JsonUtility.ToJson(SessionStats.Instance);
    }
    public SessionStats SendStatsResponse()
    {
        return CurrentStats;
    }
    #endregion
    #region GetSesstionStats
  /*  public IEnumerator GetSessionData(int vr_module_id, int patient_id)
    {
        WWWForm form = new WWWForm();
        UnityWebRequest www = UnityWebRequest.Get("https://dashboard.myvrapeutic.com/api/v1/statistics/?patient_id=" + patient_id.ToString() + "&" + "vr_module_id=" + patient_id.ToString());
        //   www.chunkedTransfer = false;
        yield return www.Send();
        Debug.Log("https://dashboard.myvrapeutic.com/api/v1/module_sessions/find_room?headset=" + patient_id.ToString() + "&" + "vr_module_id=" + vr_module_id.ToString());
        statsData = JsonUtility.FromJson<GetStatsData>(www.downloadHandler.text);
        Debug.Log(statsData);
        if (www.error != null)
        {
            Debug.Log("error" + www.error);
        }
        else if (www.downloadHandler.text != null)
        {

            Debug.Log("all" + www.downloadHandler.text);
        }

    }
    public GetStatsData SessionDataResponse()
    {
        return statsData;
    }*/
    #endregion
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
namespace OldStats
{
    public class JsonAPIS : MonoBehaviour
    {
        public Sessiondata sessionData;
        StatisticsJsonFile statisticsInstance;//for send reply
        PutRequstJson putRequstJsonInstance;// for put reply

        public UnityWebRequest www;
        public UnityWebRequest wwwPost;

        public static JsonAPIS instance;

        public event Action OnGetRequestSuccess;
        public event Action OnGetRequestFail;

        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy (this);
        }

        #region GetRequest

        public void GetRequest()
        {
            Debug.Log("get requested");
            if (sessionData.moduleId == -1)
            {
                Debug.Log("moduleId isn't obtained, kindly assign it.");
                return;
            }
            GetAndroidId();
            StartCoroutine(GetRequestIEnum());
        }

        private void GetAndroidId()
        {
            AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject contentResolver = currentActivity.Call<AndroidJavaObject>("getContentResolver");
            AndroidJavaClass secure = new AndroidJavaClass("android.provider.Settings$Secure");
            string AndroidIdLowerCase = secure.CallStatic<string>("getString", contentResolver, "android_id");
            string Android_ID = AndroidIdLowerCase.ToUpper();
            sessionData.headset = Android_ID;
            Debug.Log("sessionData.headset= " + sessionData.headset);
        }

        public IEnumerator GetRequestIEnum()
        {
            WWWForm form = new WWWForm();
            www = UnityWebRequest.Get("https://dashboard.myvrapeutic.com/api/v1/module_sessions/find_room?headset=" + sessionData.headset + "&" + "vr_module_id=" + sessionData.moduleId.ToString());
            yield return www.Send();
            //Debug.Log(https://dashboard.myvrapeutic.com/api/v1/module_sessions/find_room?headset=" + sessionData.headset + "&" + "vr_module_id=" + sessionData.moduleId.ToString());
            if (www.error != null)
            {
                Debug.Log("error Get: " + www.error);
                OnGetRequestFail();
            }
            else if (www.downloadHandler.text != null)
            {
                GetRoomIDJson getJsonInstance = JsonUtility.FromJson<GetRoomIDJson>(www.downloadHandler.text);
                sessionData.id = getJsonInstance.id;
                sessionData.roomId = getJsonInstance.room_id;
                Debug.Log("all" + www.downloadHandler.text);
                OnGetRequestSuccess();
            }

        }
        #endregion

        #region PostRequest

        public void PostRequest(object objectToBeSent)
        {
            string json = ConvertPostToJson(objectToBeSent);
            StartCoroutine(PostRequestIEnum(json));
        }

        public IEnumerator PostRequestIEnum(string json)
        {
            byte[] postData = System.Text.Encoding.UTF8.GetBytes(json);
            string url = "https://dashboard.myvrapeutic.com/api/v1/statistics";
            wwwPost = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            wwwPost.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            wwwPost.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            Debug.Log("SendjsonIEnum :" + json);
            wwwPost.SetRequestHeader("Content-Type", "application/json");
            yield return wwwPost.SendWebRequest();
            statisticsInstance = JsonUtility.FromJson<StatisticsJsonFile>(wwwPost.downloadHandler.text);
            Debug.Log("Send request Resonce code: " + wwwPost.responseCode);
            if (wwwPost.error != null)
            {
                Debug.Log("error" + wwwPost.error);
            }
            else if (wwwPost.downloadHandler.text != null)
            {

                Debug.Log("all" + wwwPost.downloadHandler.text);
            }

        }
        string ConvertPostToJson(object objectToBeSent)
        {
            return JsonUtility.ToJson(objectToBeSent);
        }
        #endregion

        #region PutRequest
        public void PutRequest()
        {
            if (sessionData.id == -1)
            {
                Debug.Log("id isn`t changed");
                return;
            }
            string json = ConvertPutTojson(sessionData.id, System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss tt"), sessionData.headset);
            StartCoroutine(PutRequestIEnum(json));
        }

        public IEnumerator PutRequestIEnum(string json)
        {
            byte[] postData = System.Text.Encoding.UTF8.GetBytes(json);
            string url = "https://dashboard.myvrapeutic.com/api/v1/module_sessions";
            string putUrl = url + "/" + sessionData.id + "?" + "ended_at=" + System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss tt") + "&" + "headset=" + sessionData.headset;
            Debug.Log("put url: " + putUrl);
            UnityWebRequest www = UnityWebRequest.Put(putUrl, postData);
            yield return www.SendWebRequest();
            Debug.Log(www.responseCode);
            putRequstJsonInstance = JsonUtility.FromJson<PutRequstJson>(www.downloadHandler.text);
            Debug.Log(putRequstJsonInstance);
            if (www.error != null)
            {
                Debug.Log("error" + www.error);
            }
            else if (www.downloadHandler.text != null)
            {
                Debug.Log("all" + www.downloadHandler.text);
                sessionData.id = -1;
                sessionData.roomId = "";
            }
        }

        string ConvertPutTojson(int id, string endedAt, string serial)
        {
            PutRequstJson putRequst = new PutRequstJson();
            putRequst.id = id;
            putRequst.ended_at = endedAt;
            putRequst.headset = serial;
            return JsonUtility.ToJson(putRequst);
        }

        public PutRequstJson ReturnPutRequstJson()
        {
            return putRequstJsonInstance;
        }
        #endregion
    }

}
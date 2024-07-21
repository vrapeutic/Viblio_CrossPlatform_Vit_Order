using System;
using System.Collections;
using System.Threading.Tasks;
using PusherClient;
using SocketIO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PusherManager : MonoBehaviour
{
#if UNITY_ANDROID
    // A mutation of https://unity3d.com/learn/tutorials/projects/2d-roguelike-tutorial/writing-game-manager
    public static PusherManager instance = null;
    private Pusher _pusher;
    private Channel _channel;
    //modified
    private const string APP_KEY = "c9287ffe04bd6fce40ff";
    //modified
    private const string APP_CLUSTER = "eu";
    public string OrgPackageName;
    string packageName;
    string serial;
    bool sameApp = false;
    //this will be controlled from NetworkManager.cs
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    async Task Start()
    {
        //#if UNITY_ANDROID
        await GetSerial();
        //await InitialisePusher();
        Console.WriteLine("Starting");
        //#endif
    }
    private async Task GetSerial()
    {
        AndroidJavaObject jo = new AndroidJavaObject("android.os.Build");
        serial = jo.GetStatic<string>("SERIAL");
        await InitialisePusher();
    }

    private async Task InitialisePusher()
    {
        //Environment.SetEnvironmentVariable("PREFER_DNS_IN_ADVANCE", "true");

        if (_pusher == null && (APP_KEY != "APP_KEY") && (APP_CLUSTER != "APP_CLUSTER"))
        {
            _pusher = new Pusher(APP_KEY, new PusherOptions()
            {
                Cluster = APP_CLUSTER
            });

            _pusher.Error += OnPusherOnError;
            _pusher.ConnectionStateChanged += PusherOnConnectionStateChanged;
            _pusher.Connected += PusherOnConnected;
            //modified
            _channel = await _pusher.SubscribeAsync("channel_" + serial);
            _channel.Subscribed += OnChannelOnSubscribed;
            await _pusher.ConnectAsync();
        }
        else
        {
            Debug.LogError("APP_KEY and APP_CLUSTER must be correctly set. Find how to set it at https://dashboard.pusher.com");
        }
    }

    private void PusherOnConnected(object sender)
    {
        Debug.Log("Connected");
        //modified
        _channel.Bind("start_module", (dynamic data) =>
        {
            Debug.Log("start_module received");
            //string jsonFile = JsonUtility.ToJson(sender);
            ListenerData listenerData = JsonUtility.FromJson<ListenerData>(Convert.ToString(data));
            Debug.Log("We Have recieved Listener massege " + listenerData.package);
            packageName = listenerData.package;
            //StartCoroutine(OpenOtherApp(listenerData.package));
            if (OrgPackageName == packageName)
            {
                sameApp = true;
                SceneManager.LoadSceneAsync("Lobby");
            }
            else
            {
                Application.Quit();
            }
            ;
        });
    }

    private void PusherOnConnectionStateChanged(object sender, ConnectionState state)
    {
        Debug.Log("Connection state changed");
    }

    private void OnPusherOnError(object s, PusherException e)
    {
        Debug.Log("Errored");
    }

    private void OnChannelOnSubscribed(object s)
    {
        Debug.Log("Subscribed");
    }

    public void Message(string message)
    {
        Debug.Log(message);
        _channel?.Trigger("time has occured", message);
    }

    async Task OnApplicationQuit()
    {
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        /**/
        AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");
        /**/
        AndroidJavaObject launchIntent = null;
        try
        {
            /**/
            // won't execute unless a different module is being open (e.g. Archeeko -> GardenDo)
            // save NetworkManager.SocketIOComp.instance.serverUrl to PlayerPref
            launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", packageName);
            ca.Call("startActivity", launchIntent);
        }
        catch (System.Exception e)
        {
            Debug.Log("can`t Open the app");
        }
        up.Dispose();
        ca.Dispose();
        packageManager.Dispose();
        launchIntent.Dispose();
        if (_pusher != null)
        {
            await _pusher.DisconnectAsync();
        }

    }
    [Serializable]
    public class ListenerData
    {
        public string room_id;
        public string package;
        public string name;
    }

    /// <summary>
    /// this function will be called two times after desktop App 
    /// </summary>
    void OnDestroy()
    {
        Debug.Log("pusher manager destroyed");
        //this the second time after pusher recieve the package name is the same app name
        if (instance.sameApp)
        {
            Debug.Log("second closed sameApp equal true");
            instance.sameApp = false;
        }
        //this the first time after desktop instance collesed
        else
        {
            Debug.Log("first collosed We will Destroy Socket IO game object");
        }
    }
#endif
}

//IEnumerator OpenOtherApp(string packge)
//{
//    yield return new WaitForSeconds(5);
//    bool fail = false;
//    /**/
//    string bundleId = packge; // your target bundle id
//    AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//    AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
//    /**/
//    AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");

//    /**/
//    AndroidJavaObject launchIntent = null;
//    try
//    {
//        /**/
//        launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", bundleId);
//    }
//    catch (System.Exception e)
//    {
//        fail = true;
//    }

//    if (fail)
//    { //open app in store
//    }
//    else //open the app
//        ca.Call("startActivity", launchIntent);
//    Application.Quit();
//    up.Dispose();
//    ca.Dispose();
//    packageManager.Dispose();
//    launchIntent.Dispose();
//}

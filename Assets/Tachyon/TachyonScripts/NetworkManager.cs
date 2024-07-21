
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SocketIO;
using System;
using System.Collections;
using SimpleTCP;
using System.Collections.Generic;

namespace Tachyon
{
    public class NetworkManager : MonoBehaviour
    {
        #region Serializable Fields
        public string nextSceneName;
        [SerializeField] string lobbySceneName;
        [SerializeField] string moduleName = "ModuleName";
        public static bool isStandalone;
        #endregion

        #region Private Members
        private VRClient client;
        private string serverUrl = "ws://1.1.1.1:3000/SocketIOComponent.instance.io/?EIO=4&transport=webSocketIOComponent.instance";
        private static JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);
        private bool isConnected = false;
        private string androidURL = "";
        private bool gotServerUrl = false;
        private static List<string> registedFunctions = new List<string>();
        #endregion

        #region Public Members
        public static NetworkManager instance = null;
        #endregion

        #region Public Functions

        public static void SetStandaloneValue(bool value)
        {
            isStandalone = value;
        }

        public static void InvokeServerMethod(string functionName, string gameObjectName, params object[] parameters)
        {
            //Debug.Log(functionName + gameObjectName);
            jsonObject.SetField("functionName", functionName + gameObjectName);
            jsonObject.SetField("gameObjectName", gameObjectName);
            AddMethodParametersToJSON(parameters);
            try
            {
                if (!isStandalone)
                    SocketIOComponent.instance.Emit(functionName + gameObjectName, jsonObject);
                else
                    LocalEventManager.InvokeLocalEvent(functionName + gameObjectName, jsonObject);
            }
            catch (NullReferenceException ex)
            {
                LocalEventManager.InvokeLocalEvent(functionName + gameObjectName, jsonObject);
            }
            catch (Exception e)
            {
                LocalEventManager.InvokeLocalEvent(functionName + gameObjectName, jsonObject); Debug.Log(e);
            }

            RemoveMethodParametersFromJSON(parameters);
        }

        public static void InvokeClientMethod(string functionName, InvokationManager invokationManager)
        {
            string functionIdentifier = functionName + invokationManager.GameObjectName();
            try
            {
                if (!isStandalone)
                {
                    jsonObject.SetField("functionName", functionIdentifier);
                    if (!registedFunctions.Contains(functionIdentifier))
                    {
                        SocketIOComponent.instance.Emit("registerFunction", jsonObject);
                        registedFunctions.Add(functionIdentifier);
                    }
                    SocketIOComponent.instance.On(functionIdentifier, invokationManager.InvokeFunction);
                }
                else
                {
                    LocalEventManager.On(functionIdentifier, invokationManager);
                }
            }
            catch (NullReferenceException ex)
            {
                LocalEventManager.On(functionIdentifier, invokationManager);
            }
            catch (Exception e)
            {
                LocalEventManager.On(functionIdentifier, invokationManager);
                Debug.Log(e);
            }
        }


        public static void RevokeClientMethod(string functionName, InvokationManager invokationManager)
        {
            //if (!isStandalone)
            //{
            //    jsonObject.SetField("functionName", functionName + invokationManager.GameObjectName());
            //    SocketIOComponent.instance.Emit("unregisterFunction", jsonObject);

            //    SocketIOComponent.instance.Off(functionName + invokationManager.GameObjectName(), invokationManager.InvokeFunction);
            //}
        }
        #endregion

        #region Private Functions
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
            else if (instance != this)
            {
                Destroy(this);
            }
        }

        void Start()
        {
            OnStart();
 //           instance.OnStartDebug();
        }

        private void OnDestroy()
        {
            Debug.Log("OnDestroy from Network Manager");
//            instance.OnStartDebug();
//            instance.RestState();
//            instance.OnStartDebug();
//        }

//        public void RestState()
//        {
//#if UNITY_ANDROID
//            ClearServerURL();
//            Debug.Log("Stopping Server...");
//            server.Stop();
//            SocketIOComponent.instance.Awake();
//            Debug.Log("Restarting Server...");
//            StartCoroutine(InitTCPServer());
//#endif
//        }
//        void OnStartDebug()
//        {
//            Debug.Log("getCurrentServerURL: " + getCurrentServerURL());
//            Debug.Log("hasServerURL: " + hasServerURL().ToString());
        }

        void OnStart()
        {
#if UNITY_ANDROID
            StartCoroutine(InitTCPServer());
            StartCoroutine(InitializeConnectionToServer());
            StartCoroutine(InitializeListeners());
#endif

#if UNITY_STANDALONE
            string url = "ws://127.0.0.1:3000/socket.io/?EIO=4&transport=websocket";
            ConnectToServer(url);
            StartCoroutine(InitializeListeners());
#endif      
        }

        public string getCurrentServerURL()
        {
            return this.serverUrl;
        }

        public bool hasServerURL()
        {
            return this.gotServerUrl;
        }

        public void ClearServerURL()
        {
            this.serverUrl = "";
            this.gotServerUrl = false;
        }

#if UNITY_ANDROID
        private SimpleTcpServer server;
        IEnumerator InitTCPServer()
        {
            yield return new WaitForSeconds(0.1f);
            // if IP is in PlayerPref
            //   then serverUrl = PlayerPref['url']
            //        ConnectToServer
            // else
            //   then open the TCP server normally
            Debug.Log("InitTCPServer");
            AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject contentResolver = currentActivity.Call<AndroidJavaObject>("getContentResolver");
            AndroidJavaClass secure = new AndroidJavaClass("android.provider.Settings$Secure");
            string AndroidIdLowerCase = secure.CallStatic<string>("getString", contentResolver, "android_id");
            string Android_ID = AndroidIdLowerCase.ToUpper();
            Debug.Log("Android ID :" + Android_ID);
            server = new SimpleTcpServer().Start(8910);
            Debug.Log("INIT TCP @ PORT 8910");
            server.DataReceived += (sender, msg) =>
            {
                //msg.Reply("Content-Type: text/plain\n\nHello from my web server!");
                //Debug.Log(msg.MessageString);
                //string[] words = msg.MessageString.Split(' ');

                if (msg.MessageString.Contains("requestInfo") )//&& !this.gotServerUrl)
                {
                    //Debug.Log("msg.MessageString :" + msg.MessageString);
                    //Debug.Log("Did not receive 'requestInfo' msg");
                    msg.Reply("headsetSerial " + Android_ID + " headsetModuleName " + PusherManager.instance.OrgPackageName);
                    //Debug.Log("headsetSerial " + Android_ID + " headsetModuleName " + PusherManager.instance.OrgPackageName);
                }
                if (msg.MessageString.Contains("IP") )//&& !this.gotServerUrl)
                {
                    string[] splitText = msg.MessageString.Split(' ');
                    string ip = splitText[1];
                    string url = "ws://" + ip + ":3000/socket.io/?EIO=4&transport=websocket";
                    this.serverUrl = url;
                    this.gotServerUrl = true;
                    //Debug.Log(ip);
                    //Debug.Log("connecting to server: " + url);
                    msg.Reply("gotServerUrl");
                }
                else
                {
                    //Debug.Log("Did not receive IP msg");
                }
            };
        }

        private IEnumerator InitializeConnectionToServer()
        {
            while (gotServerUrl == false)
            {
                yield return new WaitForSeconds(0.5f);
            }
            //
            // must be added when Hossam modifies the desktop app so that the client TCP server
            // script is being run only once when the user presses Play button
            //
            //**//
            if (isConnected == false)
            {
                Debug.Log("I got server URL, I'm gonna connect!");
                ConnectToServer(serverUrl);
            }
            else
            {
                Debug.Log("SocketIOComponent.instance.IsConnected = true");
            }
            //server.Stop();
            //Debug.Log("Server is stopped");
        }
#endif


        private IEnumerator InitializeListeners()
        {
            while (isConnected == false)
            {
                yield return new WaitForSeconds(0.5f);
            }
            #region Listeners
            SocketIOComponent.instance.On("register", OnRegister);
            SocketIOComponent.instance.On("requestModuleName", OnRequestModuleName);
            SocketIOComponent.instance.On("requestClientType", OnRequestClientType);
            SocketIOComponent.instance.On("clientDisconnected", OnClientDisconnected);
            SocketIOComponent.instance.On("connectionRejected", OnConnectionRejected);
            SocketIOComponent.instance.On("roomReady", OnRoomReady);
            #endregion
        }

        private void ConnectToServer(string url)
        {
            SocketIOComponent.instance.autoConnect = true;
            SocketIOComponent.instance.url = url;
            Debug.Log(url);
            SocketIOComponent.instance.Awake();
            SocketIOComponent.instance.Connect();
            isConnected = true;
            // Once connected -> delete serverUrl from PlayerPref
            Debug.Log("connected " + url);
        }

        private static void AddMethodParametersToJSON(object[] parameters = null)
        {
            for (int i = 0; parameters != null && i < parameters.Length; i++)
            {
                string fieldName = "param" + i.ToString();
                Type objType = parameters[i].GetType();
                if (objType == typeof(int))
                {
                    jsonObject.SetField(fieldName, (int)parameters[i]);
                }
                else if (objType == typeof(float))
                {
                    jsonObject.SetField(fieldName, (float)parameters[i]);
                }
                else if (objType == typeof(string))
                {
                    jsonObject.SetField(fieldName, (string)parameters[i]);
                }
                else if (objType == typeof(bool))
                {
                    jsonObject.SetField(fieldName, (bool)parameters[i]);
                }
            }
        }

        private static void RemoveMethodParametersFromJSON(object[] parameters = null)
        {
            for (int i = 0; parameters != null && i < parameters.Length; i++)
            {
                string fieldName = "param" + i.ToString();
                jsonObject.RemoveField(fieldName);
            }
        }

        private void OnRegister(SocketIOEvent e)
        {
            string clientId = e.data["id"].str;
            client = new VRClient(clientId);

#if UNITY_ANDROID
            client.setClientType(ClientType.Headset);
            client.setModuleName(moduleName);
#endif

#if UNITY_STANDALONE
            client.setClientType(ClientType.Desktop);
            client.setModuleName(moduleName);
#endif
        }

        private void OnRequestClientType(SocketIOEvent e)
        {
            jsonObject.SetField("clientType", client.getClientType().ToString());
            SocketIOComponent.instance.Emit("updateClientType", jsonObject);
        }

        private void OnRequestModuleName(SocketIOEvent e)
        {
            jsonObject.SetField("moduleName", client.getModuleName());
            SocketIOComponent.instance.Emit("updateModuleName", jsonObject);
        }

        public void OnClientDisconnected(SocketIOEvent e)
        {
#if UNITY_STANDALONE
             StartCoroutine(DesktopConnectionLost());
#endif

#if UNITY_ANDROID
            Debug.Log("Loading " + lobbySceneName);
            SceneManager.LoadSceneAsync(lobbySceneName);
            Debug.Log("*OnClientDisconnected  networkmanager");
            //SocketIOComponent.instance.Close();
            //SocketIOComponent.instance.Start();
#endif

        }

        IEnumerator DesktopConnectionLost()
        {
            transform.GetChild(0).gameObject.SetActive(true);
            yield return new WaitForSeconds(6);
            Application.Quit();
        }

        private void OnConnectionRejected(SocketIOEvent e)
        {
            Debug.Log("*Connection Rejected network manager");
            //SocketIOComponent.instance.Close();
        }

        private void OnRoomReady(SocketIOEvent e)
        {
            Debug.Log("Room is ready");
            SceneManager.LoadSceneAsync(nextSceneName);
        }
        #endregion

        public void DisconnectToServer()
        {
            Debug.Log("*Disconnect to server");
            SocketIOComponent.instance.Close();
            Destroy(this.gameObject);
        }

        private void OnApplicationQuit()
        {
            DisconnectToServer();
        }
    }
}
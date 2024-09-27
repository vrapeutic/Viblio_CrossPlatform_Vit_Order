using UnityEngine;

public class BridgePluginInitializer : MonoBehaviour
{
    private AndroidJavaClass unityClass;
    private AndroidJavaObject unityActivity;
    private AndroidJavaObject javaObj;

    [SerializeField] private string pluginName = "com.vrapeutic.bridge.PluginInitializer";

    private static BridgePluginInitializer instance;

    public static BridgePluginInitializer Instance { get => instance; set => instance = value; }


    private void Awake()
    {
        Instance = this;
        InitializePlugin(pluginName);
    }

    private void InitializePlugin(string pluginName)
    {
        unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
        javaObj = new AndroidJavaObject(pluginName);
        if (javaObj == null)
        {
            Debug.LogWarning("pluginInstance is equal null");
        }
        javaObj.CallStatic("setUnityActivity", unityActivity);
    }

    public void SendIntent(string data)
    {
        javaObj.Call("sendIntent", data);
    }
}

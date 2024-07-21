using System;
using System.Net.NetworkInformation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class SSIDNamePrinter : MonoBehaviour
{
    [SerializeField] Text wifiText;
    // Start is called before the first frame update
    void Start()
    {
       // StartCoroutine( GetSSIDName());
    }

    IEnumerator GetSSIDName()
    {
        yield return new WaitForSeconds(1);
        string SSID = "";
            try
            {
                using (var activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    using (var wifiManager = activity.Call<AndroidJavaObject>("getSystemService", "wifi"))
                    {
                        SSID = wifiManager.Call<AndroidJavaObject>("getConnectionInfo").Call<string>("getSSID");
                        //SSID = wifiManager.Call<AndroidJavaObject>("WifiInfo").Call<string>("getSSID");
                        wifiText.text = SSID;
                        Debug.Log("SSID Name: " + SSID);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        
    }

    public bool isWifiEnabled()
    {
        using (AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
        {
            try
            {
                using (var wifiManager = activity.Call<AndroidJavaObject>("getSystemService", "wifi"))
                {
                    return wifiManager.Call<bool>("isWifiEnabled");
                }
            }
            catch (Exception e)
            {

            }
        }
        return false;
    }

    public IEnumerator AskForPermissions()
    {
        yield return new WaitForEndOfFrame();
#if UNITY_ANDROID
        List<bool> permissions = new List<bool>() { false, false, false, false,false,false };
        List<bool> permissionsAsked = new List<bool>() { false, false, false, false, false, false };
        List<Action> actions = new List<Action>()
    {
        new Action(() => {
            permissions[0] = Permission.HasUserAuthorizedPermission("android.permission.ACCESS_WIFI_STATE");
            if (!permissions[0] && !permissionsAsked[0])
            {
                Permission.RequestUserPermission("android.permission.ACCESS_WIFI_STATE");
                permissionsAsked[0] = true;
                return;
            }
        }),
        new Action(() => {
            permissions[1] = Permission.HasUserAuthorizedPermission("android.permission.INTERNET");
            if (!permissions[1] && !permissionsAsked[1])
            {
                Permission.RequestUserPermission("android.permission.INTERNET");
                permissionsAsked[1] = true;
                return;
            }
        }),
        new Action(() => {
            permissions[2] = Permission.HasUserAuthorizedPermission("android.permission.ACCESS_NETWORK_STATE");
            if (!permissions[2] && !permissionsAsked[2])
            {
                Permission.RequestUserPermission("android.permission.ACCESS_NETWORK_STATE");
                permissionsAsked[2] = true;
                return;
            }
        }),
        new Action(() => {
            permissions[3] = Permission.HasUserAuthorizedPermission("android.permission.CHANGE_WIFI_STATE");
            if (!permissions[3] && !permissionsAsked[3])
            {
                Permission.RequestUserPermission("android.permission.CHANGE_WIFI_STATE");
                permissionsAsked[3] = true;
                return;
            }
        }),
        new Action(() => {
            permissions[4] = Permission.HasUserAuthorizedPermission("android.permission.ACCESS_FINE_LOCATION");
            if (!permissions[4] && !permissionsAsked[4])
            {
                Permission.RequestUserPermission("android.permission.ACCESS_FINE_LOCATION");
                permissionsAsked[4] = true;
                return;
            }
        }),
        new Action(() => {
            permissions[5] = Permission.HasUserAuthorizedPermission("android.permission.ACCESS_COARSE_LOCATION");
            if (!permissions[5] && !permissionsAsked[5])
            {
                Permission.RequestUserPermission("android.permission.ACCESS_COARSE_LOCATION");
                permissionsAsked[5] = true;
                return;
            }
        }),
    };
        for (int i = 0; i < permissionsAsked.Count;)
        {
            actions[i].Invoke();
            if (permissions[i])
            {
                ++i;
            }
            yield return new WaitForEndOfFrame();
        }
#endif
    }
}

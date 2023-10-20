using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SessionSceneController : MonoBehaviour
{
    bool canLoad = true;
    public void LoadMainScene()
    {
        if (!canLoad) return;
        canLoad = false;
        if (BackendSession.instance.currentRoomId != null) BackendSession.instance.StartSession();
        SceneManager.LoadSceneAsync("Main");
    }
}

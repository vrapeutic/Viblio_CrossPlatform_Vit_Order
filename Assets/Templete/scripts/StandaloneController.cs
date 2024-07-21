using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Tachyon;
public class StandaloneController : MonoBehaviour
{
    [SerializeField] BoolValue isStandalone;
    [SerializeField] bool isEnabledWhenStandalone;

    private void Awake()
    {
        if (isStandalone.Value)
        {
            if (!isEnabledWhenStandalone) gameObject.SetActive(false);
        }
        else
        {
            if (isEnabledWhenStandalone) gameObject.SetActive(false);
        }
    }

    public void OnClickingStandaloneButton()
    {
        isStandalone.Value = true;
        NetworkManager.SetStandaloneValue(isStandalone.Value);
        SceneManager.LoadSceneAsync(2);
        //SocketIOComponent.instance.Close();
        //Destroy(SocketIOComponent.instance.gameObject);
        //StartCoroutine( LoadMainSceneInum());
    }

    private IEnumerator LoadMainSceneInum()
    {
        yield return null;
        if (SocketIOComponent.instance != null) Debug.Log("Exist SocketIOComponent.instance");
        else Debug.Log("not Exist SocketIOComponent.instance"); 
        SceneManager.LoadSceneAsync(2);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadSceneAsync(2);
        //SocketIOComponent.instance.Close();
        //Destroy(SocketIOComponent.instance.gameObject);
        //StartCoroutine( LoadMainSceneInum());
    }

    private IEnumerator LoadMainSceneInum()
    {
        yield return null;
        SceneManager.LoadSceneAsync(2);
    }
}

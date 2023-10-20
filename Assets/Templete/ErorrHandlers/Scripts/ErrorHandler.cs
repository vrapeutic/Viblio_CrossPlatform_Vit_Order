using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorHandler : MonoBehaviour
{
    WaitForSeconds a3second;
    [SerializeField] GameObject connectionErrorPanel;
    // Start is called before the first frame update
    void Start()
    {
        a3second = new WaitForSeconds(3);
        transform.SetParent(Camera.main.transform);
        transform.localPosition = new Vector3(0,0,1);
        transform.localRotation = Quaternion.identity;
        StartCoroutine(CheckInternetConnection());
    }


    #region Internet connection
    IEnumerator CheckInternetConnection()
    {
        while (true)
        {
            WWW www = new WWW("https://google.com");
            yield return a3second ;
            yield return www;
            if (www.error == null)
            {
                connectionErrorPanel.SetActive(false);
            }
            else
            {
                connectionErrorPanel.SetActive(true);
            }
        }
    }
    #endregion


}

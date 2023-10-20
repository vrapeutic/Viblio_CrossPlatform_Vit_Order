using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *This script to calculate attention time for the player 
 */
public class Attention : MonoBehaviour
{
    public float AttentionTimer;
    bool canCountAttentionTimer = false;
    Renderer attentionObject1;
    Renderer attentionObject2;

    private void Awake()
    {
        #if UNITY_STANDALONE
         gameObject.SetActive(false);
        #endif
    }

    private void Start()
    {
        attentionObject1 = transform.GetChild(0).gameObject.GetComponent<Renderer>();
        attentionObject2 = transform.GetChild(1).gameObject.GetComponent<Renderer>();
    }

    private void OnEnable()
    {
#if UNITY_ANDROID
        GameManager.OnLevelBegin += StartSessionTimer;
        GameManager.OnLevelEnd += StopSessionTimer;
#endif
    }

    private void OnDisable()
    {
#if UNITY_ANDROID
        GameManager.OnLevelBegin -= StartSessionTimer;
        GameManager.OnLevelEnd -= StopSessionTimer;
#endif
    }

    public async void StartSessionTimer()
    {
        canCountAttentionTimer = true;
        AttentionTimer = 0;
        await StartSessionTimerIenum();
    }

    IEnumerator StartSessionTimerIenum()
    {
        while (canCountAttentionTimer)
        {
            if (attentionObject1.isVisible || attentionObject2.isVisible) AttentionTimer += Time.deltaTime;
            yield return null;
        }
    }

    public void StopSessionTimer()
    {
        Debug.Log("* attention timer: "+AttentionTimer);
        canCountAttentionTimer = false;
    }
}

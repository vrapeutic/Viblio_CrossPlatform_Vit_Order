using System.Collections;
using System.Collections.Generic;
using Tachyon;
using UnityEngine;
using UnityEngine.UI;

public class ShowTimer : MonoBehaviour
{
    WaitForSeconds asecound;
    bool canCount = true;

    // Start is called before the first frame update
    void Start()
    {
        InvokationManager invokationManager = new InvokationManager(this, this.gameObject.name);
        NetworkManager.InvokeClientMethod("ActivateTimerRPC", invokationManager);
        NetworkManager.InvokeClientMethod("DeActivateTimerRPC", invokationManager);
        asecound = new WaitForSeconds(1);
        GetComponentInChildren<Text>().text = Statistics.instance.closedTimeValue.ToString();
    }

    private void OnEnable()
    {
        if (!Statistics.instance.isClosedTime) Destroy(this.gameObject);
        if (!Statistics.instance.android) return;
        if (Statistics.instance.isClosedTime)
        {
            GameManager.OnLevelBegin += ActivateTimer;
            //Debug.Log(" GameManager.OnLevelBegin += ActivateTimer;");
            GameManager.OnLevelEnd += DeActivateTimer;
        }
    }

    private void OnDisable()
    {
        if (!Statistics.instance.android) return;
        if (Statistics.instance.isClosedTime)
        {
            GameManager.OnLevelBegin -= ActivateTimer;
            GameManager.OnLevelEnd -= DeActivateTimer;
        }
    }

    public void ActivateTimer()
    {
        NetworkManager.InvokeServerMethod("ActivateTimerRPC", this.gameObject.name);
    }

    public void ActivateTimerRPC()
    {
        //Debug.Log("ActivateTimerRPC");
        CounterDown();
    }

    async void CounterDown()
    {
        int currentTime = Statistics.instance.closedTimeValue;
        while (currentTime >= 0 && canCount)
        {
            UpdateText(currentTime);
            await asecound;
            currentTime--;
        }
        
    }

    void UpdateText(int value)
    {
        GetComponentInChildren<Text>().text = value.ToString();
    }

    void DeActivateTimer()
    {
        NetworkManager.InvokeServerMethod("DeActivateTimerRPC", this.gameObject.name);
    }

    public void DeActivateTimerRPC()
    {
        canCount = false;
    }
}

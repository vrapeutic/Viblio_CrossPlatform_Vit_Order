using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavingDetection : MonoBehaviour
{
    [SerializeField] GameEvent onCompletedVisitorsGreeting;
    [SerializeField] GameEvent onVisitorWave;
    GameObject target;
    int prev = -1;
    int prevPrev = -1;
    float lastTime=-1;
    bool canCheck = false;

    void OnEnable()
    {
        RaiseOnVisitorsWave();
    }

    async void RaiseOnVisitorsWave()
    {
        await new WaitForSeconds(4f);
        onVisitorWave.Raise();
    } 
        
    public void IntializeDetection()
    {
        FollowWavingHand();
        prev = -1;
        prevPrev = -1;
        lastTime = -1;
        canCheck = true;
    }

    public void FollowWavingHand()
    {
        target = GameObject.FindGameObjectWithTag("RightHand");
        transform.position = target.transform.position;//;new Vector3(target.transform.position.x,target.transform.position.y,target.transform.position.z);
        transform.rotation = target.transform.rotation;
        //Debug.Log("*t" + Time.time+ target.transform.position);
    }

    public void DetectMove(int current)//1 for right detect ,0for left detect
    {
        if (lastTime == -1) //first sensor detect
        {
            lastTime = Time.time;
            prev = current;
        }
        else
        {
            if(Time.time-lastTime>.2&& Time.time - lastTime < 0.9)//in range
            {
                if (prev == -1) prev = current;
                else if (prevPrev == -1)
                {
                    if (prev == current) IntializeDetection();
                    prevPrev = prev;
                    prev = current;
                }
                else
                {
                    if (prev == current) IntializeDetection();
                    else //3 detect with differnt hits thats mean it`s a wave 
                    {
                        canCheck = false;
                        onCompletedVisitorsGreeting.Raise();
                    }
                }
            }
            else if (Time.time - lastTime > .9)
            {
                IntializeDetection();
            }
        }
    }

    public async void DivertCorrection()
    {
        while (canCheck)
        {
            await new WaitForSeconds(3);
            if (Time.time - lastTime > 3&&canCheck) IntializeDetection();
        }
    }
}

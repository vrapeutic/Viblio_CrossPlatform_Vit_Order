﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//three types of attention determine which distractor will appear sustained(no distractor),selective(distractor with no action needed)
//and adaptive (distractor with action needed)
public class DistractorManager : MonoBehaviour
{
    [SerializeField] StringVariable typeOfAttention;
    //selective attention 
    [SerializeField] GameEvent onLibraryAnnoncementDistractor;
    [SerializeField] GameEvent onVisitorsTalking;
    [SerializeField] GameEvent onDoorDistractor;
    //adaptive attention
    [SerializeField] GameEvent onShelfFallenDistracting;
    [SerializeField] GameEvent OnRobotDistracting;
    [SerializeField] GameEvent onVisitorsGreeting;

    // Start is called before the first frame update
    void Start()
    {
        if (typeOfAttention.Value == "selective") SelectiveAttention();
        else if (typeOfAttention.Value == "adaptive") AdaptiveAttention();
    }

    async void SelectiveAttention()
    {
        await new WaitForSeconds(20);
        while (GameManager.instance.currentlyPlaying)
        {
            int rand = RandomNember();
            if (rand == 1) onLibraryAnnoncementDistractor.Raise();
            else if (rand == 2) onVisitorsTalking.Raise();
            else if (rand == 3) onDoorDistractor.Raise();
            await new WaitForSeconds(20);
        }
    }

    public async void AdaptiveAttention()
    {
        int rand = RandomNember();
        Debug.Log("AdaptiveAttention");
        await new WaitForSeconds(20);
        if (rand == 1) onShelfFallenDistracting.Raise();
        else if (rand == 2) OnRobotDistracting.Raise();
        else if (rand == 3) onVisitorsGreeting.Raise();
    }

    int RandomNember()
    {
        return Random.Range(1, 4);
    }
}
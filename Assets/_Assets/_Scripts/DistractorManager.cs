using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//three types of attention determine which distractor will appear sustained(no distractor),selective(distractor with no action needed)
//and adaptive (distractor with action needed)
public class DistractorManager : MonoBehaviour
{
    [SerializeField] StringVariable typeOfAttention;
    [SerializeField] IntVariable noOfDistractors;
    //selective attention 
    [SerializeField] GameEvent onLibraryAnnoncementDistractor;
    [SerializeField] GameEvent onVisitorsTalking;
    [SerializeField] GameEvent onDoorDistractor;
    //adaptive attention
    [SerializeField] GameEvent OnRobotDistracting;
    [SerializeField] GameEvent onShelfFallenDistracting;
    [SerializeField] GameEvent onVisitorsGreeting;
    int lastRand = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (typeOfAttention.Value == "selective") SelectiveAttention();
        else if (typeOfAttention.Value == "adaptive") AdaptiveAttention();
    }

    public void SelectiveAttention()
    {
        StartCoroutine(SelectiveAttentionIEnum());
    }
    IEnumerator SelectiveAttentionIEnum()
    {
        yield return new WaitForSeconds(30);
        while (GameManager.instance.currentlyPlaying)
        {
            int rand = RandomNember();
            if (rand == 1) onLibraryAnnoncementDistractor.Raise();
            else if (rand == 2) onVisitorsTalking.Raise();
            else if (rand == 3) onDoorDistractor.Raise();
            yield return new WaitForSeconds(30);
        }
    }

    public void AdaptiveAttention()
    {
        StartCoroutine(AdaptiveAttentionIEnum());
    }

    IEnumerator AdaptiveAttentionIEnum()
    {
        int rand = RandomNember();
        //Debug.Log("AdaptiveAttention"+rand);
        yield return new WaitForSeconds(30);
        if (GameManager.instance.currentlyPlaying)
        {
            if (rand == 1) OnRobotDistracting.Raise();
            else if (rand == 2) onShelfFallenDistracting.Raise();
            else if (rand == 3) onVisitorsGreeting.Raise();
        }
    }

    int RandomNember()
    {
        if (noOfDistractors.Value == 1) return 1;
        int maxRange=noOfDistractors.Value+1;
        int rand= Random.Range(1, maxRange);
        while (rand == lastRand)
        {
            rand = Random.Range(1, maxRange);
        }
        lastRand = rand;
        return rand;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stats;

public class Testsend : MonoBehaviour
{

    void Assign()
    {
        SendObject send1 = new SendObject();
        send1.expected_duration_in_seconds = 65f;
        /*
         Assiegn your attripute
         */
        //JsonAPIS.instance.PostRequest(send1);
    }

}

[SerializeField]
public class SendObject : StatsParent
{
    public string session_start_time;
    public string attempt_start_time;
    public string attempt_end_time;
    public float expected_duration_in_seconds = 120;
    public float actual_duration_in_seconds;
    public string level = "3";
    public string attempt_type = "open";
    public int correct_attempts = 10;
    public int wrong_attempts;
    public float impulsivity_score;
    public float response_time;
    public float omission_score;
    public float distraction_endurance_score;
    public float actual_attention_time;
}

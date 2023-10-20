using System;
using UnityEngine;
[Serializable]
public class CreateSession
{
    public int id=-1;
    public string room_id="";
    public string session_date="";
}

[Serializable]
public class SessionStats
{
    private static SessionStats instance = null;
    public static SessionStats Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SessionStats();
            }
            return instance;
        }
    }
    public string room_id;
    public MyData data = new MyData();


}

//this class will be customized to every module
[Serializable]
public class MyData 
{
    //add data attributes here 
    public string session_start_time;
    public string attempt_start_time;
    public string attempt_end_time;
    public float expected_duration_in_seconds;
    public float actual_duration_in_seconds;
    public string level;
    public string attempt_type;
    //public float total_sustained;
    //public float non_sustained;
    public int correct_attempts = 10;
    public int wrong_attempts;
    public float impulsivity_score;
    public float response_time;
    public float omission_score;
    public float distraction_endurance_score;
    public float distractibility_score;
    public float actual_attention_time;
    public float minimum_book_put_height=0f;
    public float maximum_book_put_height=0f;
    public float average_book_put_height=0f;
}

[Serializable]
public class SessionElements
{
    public string patient_id;
    public int vr_module_id;
}
[Serializable]
public class GetStatsData
{
    public SessionData sessionData ;
}
 [Serializable]
public class SessionData
{
    string desc;
}


using System;

namespace OldStats
{
    [Serializable]
    public class GetRoomIDJson //for the server reply on get request
    {
        public int id;//the sesion id
        public string room_id;
    }

    [Serializable]
    public class StatisticsJsonFile //: StatsParent
    {

        private static StatisticsJsonFile instance = null;
        public static StatisticsJsonFile Instance
        {
            get
            {
                    if (instance == null)
                    {
                        instance = new StatisticsJsonFile();
                    }
                    return instance;
            }
        }
        public string room_id;
        public JsonItems data = new JsonItems();
    }

    //this class will be customized to every module
    [Serializable]
    public class JsonItems
    {
        //add data attributes here 
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

    //to form json file put request
    [Serializable]
    public class PutRequstJson
    {
        public int id;
        public string ended_at;//the current time when set put request
        public string headset;
    }

    //for recieving pusher json file
    [Serializable]
    public class PusherJson
    {
        public string room_id;
        public string package;
        public string name;
    }
    [Serializable]
    public class Data
    {
        //add data attributes here 
        public string start_time;
        public string end_time;
        public float score;

    }


    public class StatsParent
    {
        public StatsParent()
        {
            headset = JsonAPIS.instance.sessionData.headset;
            room_id = JsonAPIS.instance.sessionData.roomId;
        }
        public string headset;
        public string room_id;
    }
}

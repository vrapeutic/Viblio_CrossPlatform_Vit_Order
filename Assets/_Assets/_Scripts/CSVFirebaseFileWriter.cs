using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVFirebaseFileWriter : MonoBehaviour
{
    CSVFileManager myCSVFileManager;
    string header, filename, dateTime, timeString,data="";
    public void SaveScvfile()
    {
        myCSVFileManager = new CSVFileManager();
        header = "session_start_time, attempt_start_time, attempt_end_time, expected_duration_in_seconds, "+
            "actual_duration_in_seconds, level, attempt_type, correct_attempts, wrong_attempts, "+
            "impulsivity_score, response_time, omission_score, distraction_endurance_score, distractibility_score, "+
            "actual_attention_time, minimum_book_put_height, maximum_book_put_height, average_book_put_height";

        data = SessionStats.Instance.data.session_start_time+", "+SessionStats.Instance.data.attempt_start_time+", "+SessionStats.Instance.data.attempt_end_time+", "+SessionStats.Instance.data.expected_duration_in_seconds+", " +
            SessionStats.Instance.data.actual_duration_in_seconds+", "+SessionStats.Instance.data.level+", "+SessionStats.Instance.data.attempt_type+", "+SessionStats.Instance.data.correct_attempts+", "+SessionStats.Instance.data.wrong_attempts+", " +
            SessionStats.Instance.data.impulsivity_score+", "+SessionStats.Instance.data.response_time+", "+SessionStats.Instance.data.omission_score+", "+SessionStats.Instance.data.distraction_endurance_score+", "+SessionStats.Instance.data.distractibility_score+", " +
            SessionStats.Instance.data.actual_attention_time+", "+SessionStats.Instance.data.minimum_book_put_height+", "+SessionStats.Instance.data.maximum_book_put_height+", "+SessionStats.Instance.data.average_book_put_height;
        dateTime = DateTime.Now.ToString("dd-MM-yyyy");
        timeString = DateTime.Now.ToString("hh-mm-ss");
        filename = dateTime + "-" + timeString+"cognitiveData.csv";
        try
        {
            myCSVFileManager.writeStringToFile(header + data, filename);
        }
        catch (Exception)
        {
            Debug.Log("can`t write to csv file: ");
        }
    }
}

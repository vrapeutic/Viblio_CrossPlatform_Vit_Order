using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
/* in this app checking for correct attempt is tricky acording to phisics engine so we double collidiers
 * so it takes about half seconf so all stats have a half second delay
 */
public class Stats : MonoBehaviour
{
    [SerializeField] IntVariable level;
    //times when we can start aiming or end for one target
    List<System.DateTime> startingTimes = new List<System.DateTime>();
    List<System.DateTime> endingTimes = new List<System.DateTime>();
    //ending time -starting time
    List<double> interruptionDurations = new List<double>();
    //for following Distractors
    List<string> DistractorsName = new List<string>();
    List<double> TimeFollowingDistractors = new List<double>();
    System.DateTime registerDistractorTime;
    [SerializeField] FloatVariable timeFollowingSelectiveDistractor;
    string collectedData = "";
    [SerializeField] StringVariable fileName;
    private void Start()
    {
        Debug.Log(System.DateTime.Now.ToString());
        RegisteringStartTimeForTargeting();

    }

    public void RegisteringStartTimeForTargeting()
    {
        if (!GameManager.instance.currentlyPlaying) return;
        startingTimes.Add(System.DateTime.Now);
        Debug.Log("StartTimeForTargeting :" + startingTimes[startingTimes.Count - 1]);
    }

    public void RegisteringEndTimeForTargeting()
    {
        endingTimes.Add(System.DateTime.Now);
        Debug.Log("EndTimeForTargeting :" + endingTimes[endingTimes.Count - 1]);
    }

    public void RegisteringInterraptions()
    {
        for (int i = 0; i < endingTimes.Count; i++)
        {
            interruptionDurations.Add((endingTimes[i] - startingTimes[i]).TotalSeconds);
            Debug.Log("Interraptions " + (interruptionDurations.Count - 1) + ":" + interruptionDurations[interruptionDurations.Count - 1]);
        }
    }

    //if we are selective we will optain the distracting time from camera hitted the distractor
    //if we are adaptive we will optain the distracting time from difference between raise distractor event and complation of distractor
    public void RegisteringDistractorName(string name)
    {
        DistractorsName.Add(name);
        registerDistractorTime = System.DateTime.Now;
        if (name == "Visitors_talking" || name == "Library_annoncement" || name == "Door_opening")
        {
            StartCoroutine(RegisteringDistractorFollowingTimeIenum());
        }
        Debug.Log("RegisteringDistractorName: " + name);
    }
    IEnumerator RegisteringDistractorFollowingTimeIenum()
    {
        yield return new WaitForSeconds(10);
        TimeFollowingDistractors.Add(timeFollowingSelectiveDistractor.Value);
        Debug.Log("DistractorFollowingTime " + DistractorsName[TimeFollowingDistractors.Count - 1] + ":" + TimeFollowingDistractors[TimeFollowingDistractors.Count - 1]);
    }

    public void RegisteringDistractorFollowingTime()
    {
        TimeFollowingDistractors.Add((System.DateTime.Now - registerDistractorTime).TotalSeconds);
        Debug.Log("DistractorFollowingTime " + DistractorsName[TimeFollowingDistractors.Count - 1] + ":" + TimeFollowingDistractors[TimeFollowingDistractors.Count - 1]);
    }

    public void WriteCSV()
    {
        collectedData += "Viblio" + ", " + level.Value + Environment.NewLine;
        //Debug.Log("!!!collectedData1 :" + collectedData);
        collectedData += "Target Starting Time" + ", " + "Target Hitting Time " + ", " + "Interruption Durations" + ", " +
            "Distractor Name          " + ", " + "Time Following It" + Environment.NewLine;
       // Debug.Log("!!!collectedData2 :" + collectedData);
        int arrLength = endingTimes.Count > DistractorsName.Count ? endingTimes.Count : DistractorsName.Count;
        Debug.Log("!!!arrLength: " + arrLength + " DistractorsName.Count " + DistractorsName.Count + " endingTimes.Count " + endingTimes.Count);
        for (int i = 0; i < arrLength; i++)
        {
            if (i < endingTimes.Count && i < TimeFollowingDistractors.Count)
                collectedData += startingTimes[i].ToString() + ", " + endingTimes[i].ToString() + ", " + interruptionDurations[i].ToString() + ", " +
                    DistractorsName[i].ToFixedString(25, ' ') + ", " + TimeFollowingDistractors[i].ToString() + Environment.NewLine;
            else if (i < endingTimes.Count) collectedData += startingTimes[i].ToString() + ", " + endingTimes[i].ToString() + ", " + interruptionDurations[i].ToString() + Environment.NewLine;
            else if (i < TimeFollowingDistractors.Count) collectedData += " , , , " + DistractorsName[i].ToFixedString(25, ' ') + ", " + TimeFollowingDistractors[i].ToString() + Environment.NewLine;
        }
        //Debug.Log("!!!collectedData3 :" + collectedData);
        CSVWriter csv = new CSVWriter();
        GetComponent<CSVWriter>().WriteCSV(collectedData, fileName.Value);
        Debug.Log("!!WriteCSV");

    }
}

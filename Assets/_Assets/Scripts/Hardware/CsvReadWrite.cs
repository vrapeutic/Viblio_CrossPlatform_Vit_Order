using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using UnityEditor;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Runtime.InteropServices;
public class CsvReadWrite : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData) { }
    string FileName = "";
    TextWriter twAvg;
    TextWriter twFull;
    TextWriter twGrahp;
    TextWriter twQuat;
    TextWriter twRec;
    DateTime StartDateTime;
    DateTime EndDateTime;
    DateTime epochStart;
    string dateTime;
    int cur_time;
    bool dataSent;
    // Use this for initialization
    void Start()
    {
        epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        cur_time = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
        StartDateTime = DateTime.Now;
        dateTime = StartDateTime.ToString("dd-MM-yyyy");
    }
    public void SaveFile()
    {
        FileName = "csvdata.csv";
        twAvg = new StreamWriter(FileName, true);
        twRec = new StreamWriter(FileName+"Rec.csv", true);
        twFull = new StreamWriter(FileName+"full.csv", true);
        twGrahp = new StreamWriter("Graphdata.csv", false);
        twQuat = new StreamWriter("Quatdata.csv", false);
        dataSent = true;
        WriteCSV();
        twAvg.Close();
        twFull.Close();
        twRec.Close();
        Writedata();
        twGrahp.Close();
        twQuat.Close();
        python();
        File.Delete ("Grahpdata.csv");
    }
    public void WriteCSV()
    {
        twAvg.WriteLine("sensor number, avg roll, avg pitch, avg yaw, avg roll speed, avg pitch speed, avg yaw speed, avg acc x, avg acc y, avg acc z");
        twAvg.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_R/Shoulder_R/ShoulderPart1_R/ShoulderPart2_R/ShoulderPart3_R/Elbow_R/ElbowPart1_R/ElbowPart2_R/ElbowPart3_R/Wrist_R").GetComponent<SensorListener>().avrData);
        twAvg.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_R/Shoulder_R/ShoulderPart1_R/ShoulderPart2_R/ShoulderPart3_R/Elbow_R").GetComponent<SensorListener>().avrData);
        twAvg.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_R/Shoulder_R").GetComponent<SensorListener>().avrData);
        twAvg.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M").GetComponent<SensorListener>().avrData);
        twAvg.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_L/Shoulder_L").GetComponent<SensorListener>().avrData);
        twAvg.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/ShoulderPart3_L/Elbow_L").GetComponent<SensorListener>().avrData);
        twAvg.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/ShoulderPart3_L/Elbow_L/ElbowPart1_L/ElbowPart2_L/ElbowPart3_L/Wrist_L").GetComponent<SensorListener>().avrData);
        twAvg.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/Hip_R/HipPart1_R/HipPart2_R/HipPart3_R/Knee_R/KneePart1_R/KneePart2_R/KneePart3_R/Ankle_R").GetComponent<SensorListener>().avrData);
        twAvg.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/Hip_R/HipPart1_R/HipPart2_R/HipPart3_R/Knee_R").GetComponent<SensorListener>().avrData);
        twAvg.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/Hip_R").GetComponent<SensorListener>().avrData);
        twAvg.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/Hip_L").GetComponent<SensorListener>().avrData);
        twAvg.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/Hip_L/HipPart1_L/HipPart2_L/HipPart3_L/Knee_L").GetComponent<SensorListener>().avrData);
        twAvg.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/Hip_L/HipPart1_L/HipPart2_L/HipPart3_L/Knee_L/KneePart1_L/KneePart2_L/KneePart3_L/Ankle_L").GetComponent<SensorListener>().avrData);

        twFull.WriteLine("sensor number, roll, pitch, yaw, roll speed, pitch speed, yaw speed, acc x, acc y, acc z");
        twFull.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_R/Shoulder_R/ShoulderPart1_R/ShoulderPart2_R/ShoulderPart3_R/Elbow_R/ElbowPart1_R/ElbowPart2_R/ElbowPart3_R/Wrist_R").GetComponent<SensorListener>().sensorData);
        twFull.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_R/Shoulder_R/ShoulderPart1_R/ShoulderPart2_R/ShoulderPart3_R/Elbow_R").GetComponent<SensorListener>().sensorData);
        twFull.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_R/Shoulder_R").GetComponent<SensorListener>().sensorData);
        twFull.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M").GetComponent<SensorListener>().sensorData);
        twFull.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_L/Shoulder_L").GetComponent<SensorListener>().sensorData);
        twFull.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/ShoulderPart3_L/Elbow_L").GetComponent<SensorListener>().sensorData);
        twFull.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/ShoulderPart3_L/Elbow_L/ElbowPart1_L/ElbowPart2_L/ElbowPart3_L/Wrist_L").GetComponent<SensorListener>().sensorData);
        twFull.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/Hip_R/HipPart1_R/HipPart2_R/HipPart3_R/Knee_R/KneePart1_R/KneePart2_R/KneePart3_R/Ankle_R").GetComponent<SensorListener>().sensorData);
        twFull.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/Hip_R/HipPart1_R/HipPart2_R/HipPart3_R/Knee_R").GetComponent<SensorListener>().sensorData);
        twFull.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/Hip_R").GetComponent<SensorListener>().sensorData);
        twFull.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/Hip_L").GetComponent<SensorListener>().sensorData);
        twFull.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/Hip_L/HipPart1_L/HipPart2_L/HipPart3_L/Knee_L").GetComponent<SensorListener>().sensorData);
        twFull.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/Hip_L/HipPart1_L/HipPart2_L/HipPart3_L/Knee_L/KneePart1_L/KneePart2_L/KneePart3_L/Ankle_L").GetComponent<SensorListener>().sensorData);
    
        twRec.WriteLine("sensor number, min roll, min pitch, min yaw, max roll, max pitch, max yaw, min roll speed, min pitch speed, min yaw speed, max roll speed, max pitch speed, max yaw speed, min acc x, min acc y, min acc z, max acc x, max acc y, max acc z");
        twRec.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_R/Shoulder_R/ShoulderPart1_R/ShoulderPart2_R/ShoulderPart3_R/Elbow_R/ElbowPart1_R/ElbowPart2_R/ElbowPart3_R/Wrist_R").GetComponent<SensorListener>().recordData);
        twRec.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_R/Shoulder_R/ShoulderPart1_R/ShoulderPart2_R/ShoulderPart3_R/Elbow_R").GetComponent<SensorListener>().recordData);
        twRec.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_R/Shoulder_R").GetComponent<SensorListener>().recordData);
        twRec.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M").GetComponent<SensorListener>().recordData);
        twRec.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_L/Shoulder_L").GetComponent<SensorListener>().recordData);
        twRec.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/ShoulderPart3_L/Elbow_L").GetComponent<SensorListener>().recordData);
        twRec.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/ShoulderPart3_L/Elbow_L/ElbowPart1_L/ElbowPart2_L/ElbowPart3_L/Wrist_L").GetComponent<SensorListener>().recordData);
        twRec.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/Hip_R/HipPart1_R/HipPart2_R/HipPart3_R/Knee_R/KneePart1_R/KneePart2_R/KneePart3_R/Ankle_R").GetComponent<SensorListener>().recordData);
        twRec.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/Hip_R/HipPart1_R/HipPart2_R/HipPart3_R/Knee_R").GetComponent<SensorListener>().recordData);
        twRec.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/Hip_R").GetComponent<SensorListener>().recordData);
        twRec.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/Hip_L").GetComponent<SensorListener>().recordData);
        twRec.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/Hip_L/HipPart1_L/HipPart2_L/HipPart3_L/Knee_L").GetComponent<SensorListener>().recordData);
        twRec.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/Hip_L/HipPart1_L/HipPart2_L/HipPart3_L/Knee_L/KneePart1_L/KneePart2_L/KneePart3_L/Ankle_L").GetComponent<SensorListener>().recordData);
    }
    void Writedata()
    {
        twGrahp.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_R/Shoulder_R/ShoulderPart1_R/ShoulderPart2_R/ShoulderPart3_R/Elbow_R/ElbowPart1_R/ElbowPart2_R/ElbowPart3_R/Wrist_R").GetComponent<SensorListener>().avrData);
        twGrahp.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_R/Shoulder_R/ShoulderPart1_R/ShoulderPart2_R/ShoulderPart3_R/Elbow_R").GetComponent<SensorListener>().avrData);
        twGrahp.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_R/Shoulder_R").GetComponent<SensorListener>().avrData);
        twGrahp.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M").GetComponent<SensorListener>().avrData);
        twGrahp.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_L/Shoulder_L").GetComponent<SensorListener>().avrData);
        twGrahp.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/ShoulderPart3_L/Elbow_L").GetComponent<SensorListener>().avrData);
        twGrahp.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/ShoulderPart3_L/Elbow_L/ElbowPart1_L/ElbowPart2_L/ElbowPart3_L/Wrist_L").GetComponent<SensorListener>().avrData);
        twGrahp.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/Hip_R/HipPart1_R/HipPart2_R/HipPart3_R/Knee_R/KneePart1_R/KneePart2_R/KneePart3_R/Ankle_R").GetComponent<SensorListener>().avrData);
        twGrahp.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/Hip_R/HipPart1_R/HipPart2_R/HipPart3_R/Knee_R").GetComponent<SensorListener>().avrData);
        twGrahp.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/Hip_R").GetComponent<SensorListener>().avrData);
        twGrahp.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/Hip_L").GetComponent<SensorListener>().avrData);
        twGrahp.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/Hip_L/HipPart1_L/HipPart2_L/HipPart3_L/Knee_L").GetComponent<SensorListener>().avrData);
        twGrahp.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/Hip_L/HipPart1_L/HipPart2_L/HipPart3_L/Knee_L/KneePart1_L/KneePart2_L/KneePart3_L/Ankle_L").GetComponent<SensorListener>().avrData);

        twQuat.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_R/Shoulder_R/ShoulderPart1_R/ShoulderPart2_R/ShoulderPart3_R/Elbow_R/ElbowPart1_R/ElbowPart2_R/ElbowPart3_R/Wrist_R").GetComponent<SensorListener>().quatData);
        twQuat.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_R/Shoulder_R/ShoulderPart1_R/ShoulderPart2_R/ShoulderPart3_R/Elbow_R").GetComponent<SensorListener>().quatData);
        twQuat.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_R/Shoulder_R").GetComponent<SensorListener>().quatData);
        twQuat.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M").GetComponent<SensorListener>().quatData);
        twQuat.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_L/Shoulder_L").GetComponent<SensorListener>().quatData);
        twQuat.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/ShoulderPart3_L/Elbow_L").GetComponent<SensorListener>().quatData);
        twQuat.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/RootPart1_M/RootPart2_M/Spine1_M/Spine1Part1_M/Spine1Part2_M/Chest_M/Scapula_L/Shoulder_L/ShoulderPart1_L/ShoulderPart2_L/ShoulderPart3_L/Elbow_L/ElbowPart1_L/ElbowPart2_L/ElbowPart3_L/Wrist_L").GetComponent<SensorListener>().quatData);
        twQuat.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/Hip_R/HipPart1_R/HipPart2_R/HipPart3_R/Knee_R/KneePart1_R/KneePart2_R/KneePart3_R/Ankle_R").GetComponent<SensorListener>().quatData);
        twQuat.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/Hip_R/HipPart1_R/HipPart2_R/HipPart3_R/Knee_R").GetComponent<SensorListener>().quatData);
        twQuat.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/Hip_R").GetComponent<SensorListener>().quatData);
        twQuat.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/Hip_L").GetComponent<SensorListener>().quatData);
        twQuat.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/Hip_L/HipPart1_L/HipPart2_L/HipPart3_L/Knee_L").GetComponent<SensorListener>().quatData);
        twQuat.WriteLine(GameObject.Find("/HuisseinIMURig/Group/Main/DeformationSystem/Root_M/Hip_L/HipPart1_L/HipPart2_L/HipPart3_L/Knee_L/KneePart1_L/KneePart2_L/KneePart3_L/Ankle_L").GetComponent<SensorListener>().quatData);
    }
    public void python(){
        ProcessStartInfo pythonInfo = new ProcessStartInfo ();
        Process python;
        pythonInfo.FileName=@"python.exe";
        pythonInfo.Arguments=@"grahper.pyc";
        pythonInfo.CreateNoWindow = false;
        pythonInfo.UseShellExecute = false;
        python = Process.Start (pythonInfo);
        python.WaitForExit ();
    }
    public void OnApplicationQuit()
    {
        EndDateTime = DateTime.Now;
        if (!dataSent)
        {
            twAvg = new StreamWriter(FileName, true);
            twRec = new StreamWriter(FileName+"Rec.csv", true);
            twFull = new StreamWriter(FileName+"full.csv", true);
            twGrahp = new StreamWriter("Graphdata.csv", false);
            twQuat = new StreamWriter("Quatdata.csv", false);
            WriteCSV();
            twAvg.Close();
            twFull.Close();
            twRec.Close();
            Writedata();
            twGrahp.Close();
            twQuat.Close();
        }
    }
}

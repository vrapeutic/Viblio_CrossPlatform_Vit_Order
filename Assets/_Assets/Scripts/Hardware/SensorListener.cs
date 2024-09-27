using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;


public class SensorListener : MonoBehaviour{

    public string sensorId = "00";
    public Boolean left = false;
    public Boolean right = false;
    public Boolean up = false;

    string ip;
    int port;
    static readonly int framesize = 84;
    TcpClient socketConnection;
    NetworkStream stream;
    Thread clientReceiveThread;

    Boolean reconnect = false;
    Queue<Quaternion> quatBuffer = new Queue<Quaternion>();
    Queue<Vector3> accBuffer = new Queue<Vector3>();
    Quaternion originRotation;
    Vector3 oldAngles;
    //CSVFileManager myCSVFileManager;


    IEnumerator sensorReconnect(int secs){
        
        reconnect = false;
        quatBuffer.Clear();
        accBuffer.Clear();
        if(clientReceiveThread != null){
            clientReceiveThread.Abort();
        }
        if(socketConnection != null){
            socketConnection.GetStream().Close();
            socketConnection.Client.Close();
            socketConnection.Close();
        }
        yield return new WaitForSeconds(secs);
        System.GC.Collect();
        clientReceiveThread = new Thread(new ThreadStart(ListenForData));
        clientReceiveThread.Start();

    }


    // Use this for initialization 	
    void Start(){

        ip = "172.24.60." + (254 - Int32.Parse(sensorId, System.Globalization.NumberStyles.HexNumber));
        port = 9000 + Int32.Parse(sensorId, System.Globalization.NumberStyles.HexNumber);
        originRotation = transform.rotation;
        oldAngles = transform.localEulerAngles;
        clientReceiveThread = new Thread(new ThreadStart(ListenForData));
        clientReceiveThread.Start();
        //myCSVFileManager.writeStringToFile("testttttttt", "test.csv");

    }


    /// Runs in background clientReceiveThread; Listens for incomming data. 	
    private void ListenForData(){

        Quaternion quat = new Quaternion();
        Byte[] bytes = new Byte[framesize];
        int length;
        int n, btn;
        float w, x, y, z, ax, ay, az, batt;

        try{ 
            socketConnection = new TcpClient(ip, port);
        }
        catch (SocketException socketException){
            Debug.Log("Socket exception: " + socketException);
            reconnect = true;
            //while (true) ;
            return;

        }
        Debug.Log("***ana matafltch");
        while (socketConnection.Connected == true){
            stream = socketConnection.GetStream();
            while ((length = stream.Read(bytes, 0, bytes.Length)) != 0){
                var incommingData = new byte[length];
                Array.Copy(bytes, 0, incommingData, 0, length);
                string serverMessage = Encoding.ASCII.GetString(incommingData);
                serverMessage = serverMessage.Replace("\"", "");
                //Debug.Log(serverMessage);
                char[] delimiterChars = { ' ' };
                string[] words =  serverMessage.Split(delimiterChars);
                try{ 
                    n = int.Parse(words[0]);
                    w = float.Parse(words[1]);
                    x = float.Parse(words[2]);
                    y = float.Parse(words[3]);
                    z = float.Parse(words[4]);
                    ax = float.Parse(words[5]);
                    ay = float.Parse(words[6]);
                    az = float.Parse(words[7]);
                    batt = float.Parse(words[8]);
                    btn = int.Parse(words[9]);
                    //Debug.Log("datasent of 0x" + n + " : " + x + " " + y + " " + z + " " + w + " " + ax + " " + ay + " " + az + " " + batt + " " + btn);
                    if(up){
                        if (left){
                            quat = new Quaternion(x, y, -z, -w);
                        }
                        else if (right){
                            quat = new Quaternion(x, -y, -z, w);
                        }
                        else{
                            quat = new Quaternion(z, -x, -y, w);
                        }
                    }
                    else{
                        if (left){
                            quat = new Quaternion(z, x, y, w);
                        }
                        else if (right){
                            quat = new Quaternion(-z, -x, y, w);
                        }
                        else{
                            quat = new Quaternion(z, -x, -y, w);
                        }
                    }
                    quatBuffer.Enqueue(quat);
                    accBuffer.Enqueue(new Vector3(ax * 1000, ay * 1000, az * 1000));
                }
                catch (Exception e){
                    Debug.Log("{0} Exception caught." + e.ToString());
                }
            }
        }
        reconnect = true;
        return;

    }


    Vector3 acc;
    Vector3 angles;
    Vector3 angulerVel;
    public String sensorData = "";
    public String avrData = "";
    public String recordData = "";
    public String quatData = "";
    //public String accData = "";
    Vector3 minAng = new Vector3(360, 360, 360);
    Vector3 maxAng = new Vector3(-180, -180, -180);
    Vector3 avrAng = new Vector3(0, 0, 0);
    Vector3 minVel = new Vector3(99999, 99999, 99999);
    Vector3 maxVel = new Vector3(0, 0, 0);
    Vector3 avrVel = new Vector3(0, 0, 0);
    Vector3 minAcc = new Vector3(99999, 99999, 99999);
    Vector3 maxAcc = new Vector3(0, 0, 0);
    Vector3 avrAcc = new Vector3(0, 0, 0);
    int cnt = 0;
    int accCnt = 0;
    float sumAccX = 0;
    float sumAccY = 0;
    //int dataCnt = 0;
    int jumpcnt = 0;
    int accWatchdogCntX = 0;
    int accWatchdogCntY = 0;
    Boolean jumplock = true;
    Boolean accPosLockX = true;
    Boolean accNegLockX = true;
    Boolean accPosFlagX = false;
    Boolean accNegFlagX = false;
    Boolean accPosLockY = true;
    Boolean accNegLockY = true;
    Boolean accPosFlagY = false;
    Boolean accNegFlagY = false;
    //static readonly int accPoints = 100;
    static readonly int avrFactor = 50;
    static readonly int accWind = 5;
    static readonly int accThesX = 250;
    static readonly int accThesY = 500;
    //TextWriter tw;

    // Update is called once per frame
    void Update() {

        if(quatBuffer.Count != 0 && accBuffer.Count != 0 && !reconnect){
            
            transform.rotation = originRotation * quatBuffer.Dequeue();
            //Debug.Log("quat of 0x" + sensorId + " : " + transform.rotation);
            acc = accBuffer.Dequeue();
            //Debug.Log("Acc of 0x" + sensorId + " : " + acc + gravity);
            angles = transform.localEulerAngles - new Vector3(270, 270, 270);
            angles.x = angles.x <= 0 ? angles.x + 360 : angles.x;
            angles.y = angles.y <= 0 ? angles.y + 360 : angles.y;
            angles.z = angles.z <= 0 ? angles.z + 360 : angles.z;
            //Debug.Log("angles of " + sensorId + " : " + angles);
            /*angles = transform.localEulerAngles;
            angles.x = angles.x > 180 ? angles.x - 360 : angles.x;
            angles.y = angles.y > 180 ? angles.y - 360 : angles.y;
            angles.z = angles.z > 180 ? angles.z - 360 : angles.z;*/
            angulerVel = (angles - oldAngles) / Time.deltaTime;
            angulerVel /= 30;
            angulerVel.x = Math.Abs(angulerVel.x) > 1000 ? 0 : angulerVel.x;
            angulerVel.y = Math.Abs(angulerVel.y) > 1000 ? 0 : angulerVel.y;
            angulerVel.z = Math.Abs(angulerVel.z) > 1000 ? 0 : angulerVel.z;
            quatData += transform.rotation.w + ", " + transform.rotation.x + ", " + transform.rotation.y + ", " + transform.rotation.z + "\n";
            sensorData += sensorId + ", " + angles + ", " + angulerVel + ", " + acc + "\n";
            recordData = sensorId + ", " + minAng + ", " + maxAng + ", " + minVel + ", " + maxVel + ", " + minAcc + ", " + maxAcc + "\n";
            //Debug.Log(recordData);
            if(cnt > avrFactor){
                avrAng /= avrFactor;
                avrVel /= avrFactor;
                avrAcc /= avrFactor;
                avrData += sensorId + ", " + avrAng + ", " + avrVel + ", " + avrAcc + "\n";
                cnt = 0;
                avrAng = new Vector3(0, 0, 0);
                avrVel = new Vector3(0, 0, 0);
                avrAcc = new Vector3(0, 0, 0);
            }
            else{
                avrAng += angles;
                avrVel += angulerVel;
                avrAcc += acc;
                minAng.x = minAng.x < angles.x ? minAng.x : angles.x;
                minAng.y = minAng.y < angles.y ? minAng.y : angles.y;
                minAng.z = minAng.z < angles.z ? minAng.z : angles.z;
                minVel.x = minVel.x < angulerVel.x ? minVel.x : angulerVel.x;
                minVel.y = minVel.y < angulerVel.y ? minVel.y : angulerVel.y;
                minVel.z = minVel.z < angulerVel.z ? minVel.z : angulerVel.z;
                minAcc.x = minAcc.x < acc.x ? minAcc.x : acc.x;
                minAcc.y = minAcc.y < acc.y ? minAcc.y : acc.y;
                minAcc.z = minAcc.z < acc.z ? minAcc.z : acc.z;
                maxAng.x = maxAng.x > angles.x ? maxAng.x : angles.x;
                maxAng.y = maxAng.y > angles.y ? maxAng.y : angles.y;
                maxAng.z = maxAng.z > angles.z ? maxAng.z : angles.z;
                maxVel.x = maxVel.x > angulerVel.x ? maxVel.x : angulerVel.x;
                maxVel.y = maxVel.y > angulerVel.y ? maxVel.y : angulerVel.y;
                maxVel.z = maxVel.z > angulerVel.z ? maxVel.z : angulerVel.z;
                maxAcc.x = maxAcc.x > acc.x ? maxAcc.x : acc.x;
                maxAcc.y = maxAcc.y > acc.y ? maxAcc.y : acc.y;
                maxAcc.z = maxAcc.z > acc.z ? maxAcc.z : acc.z;
                cnt++;
            }
            if(sensorId.Equals("3")){

                if(acc.z > 700 && !jumplock){
                    jumplock = true;
                }
                if(acc.z < -250 && jumplock){
                    jumplock = false;
                    jumpcnt += 1;
                    Debug.Log("jmp cnt: " + jumpcnt);
                    accCnt = 0;
                    sumAccX = 0;
                    sumAccY = 0;
                    accNegFlagY = false;
                    accPosFlagY = false;
                    accNegLockY = true;
                    accPosLockY = true;
                    accWatchdogCntY = 0;
                    accNegFlagX = false;
                    accPosFlagX = false;
                    accNegLockX = true;
                    accPosLockX = true;
                    accWatchdogCntX = 0;
                    try{
                        GameObject.Find("/Huissein rig").GetComponent<Jump>().moVertical();
                    }
                    catch(NullReferenceException e){
                        e.ToString();
                    }
                    try{
                        GameObject.Find("/reem rig").GetComponent<Jump>().moVertical();
                    }
                    catch(NullReferenceException e){
                        e.ToString();
                    }
                }
                if(acc.z > 700 && jumplock){
                    if(accCnt < accWind){
                        sumAccX += acc.x;
                        sumAccY += acc.y;
                        accCnt += 1;
                        /*if(dataCnt < accPoints){
                            dataCnt += 1;
                            accData += "\n" + acc + ", " + "0" + ", " + "0";
                        }
                        else{
                            tw = new StreamWriter("C:\\Users\\A7\\Documents\\d_h.csv", true);
                            tw.WriteLine(accData);
                            tw.Close();
                            Application.Quit();
                            UnityEditor.EditorApplication.isPlaying = false;
                        }*/
                    }
                    else{
                        //Debug.Log( "accsumy" + sumAccY);
                        if (sumAccX < -accThesX && accNegLockX){
                            //Debug.Log( "accsumx" + sumAccX);
                            if (accPosFlagX){
                                try{
                                    GameObject.Find("/Cube").GetComponent<DirDecCube>().moVertical(true);
                                }
                                catch(NullReferenceException e){
                                    e.ToString();
                                }
                                accNegFlagX = false;
                                accPosFlagX = false;
                                accNegLockX = false;
                            }
                            else{
                                accNegFlagX = true;
                                accPosLockX = true;
                            }
                            accWatchdogCntX = 0;
                        }
                        else if(sumAccX > accThesX && accPosLockX){
                            //Debug.Log( "accsumx" + sumAccX);
                            if (accNegFlagX){
                                try{
                                    GameObject.Find("/Cube").GetComponent<DirDecCube>().moVertical(false);
                                }
                                catch(NullReferenceException e){
                                    e.ToString();                                
                                }
                                accNegFlagX = false;
                                accPosFlagX = false;
                                accPosLockX = false;
                            }
                            else{
                                accPosFlagX = true;
                                accNegLockX = true;
                            }
                            accWatchdogCntX = 0;
                        }
                        else{
                            accWatchdogCntX += 1;
                        }
                        if (accWatchdogCntX >= accWind){
                            accNegFlagX = false;
                            accPosFlagX = false;
                            accNegLockX = true;
                            accPosLockX = true;
                            accWatchdogCntX = 0;
                        }

                        if (sumAccY < -accThesY && accNegLockY){
                            if (accPosFlagY){
                                try{
                                    GameObject.Find("/Cube").GetComponent<DirDecCube>().moHoriznotal(true);
                                }
                                catch(NullReferenceException e){
                                    e.ToString();                                
                                }
                                accNegFlagY = false;
                                accPosFlagY = false;
                                accNegLockY = false;
                            }
                            else{
                                accNegFlagY = true;
                                accPosLockY = true;
                            }
                            accWatchdogCntY = 0;
                        }
                        else if(sumAccY > accThesY && accPosLockY){
                            if (accNegFlagY){
                                try{
                                    GameObject.Find("/Cube").GetComponent<DirDecCube>().moHoriznotal(false);
                                }
                                catch(NullReferenceException e){
                                    e.ToString();
                                }
                                accNegFlagY = false;
                                accPosFlagY = false;
                                accPosLockY = false;
                            }
                            else{
                                accPosFlagY = true;
                                accNegLockY = true;
                            }
                            accWatchdogCntY = 0;
                        }
                        else{
                            accWatchdogCntY += 1;
                        }
                        if (accWatchdogCntY >= accWind){
                            accNegFlagY = false;
                            accPosFlagY = false;
                            accNegLockY = true;
                            accPosLockY = true;
                            accWatchdogCntY = 0;
                        }

                        //accData += ", " + sumAccX + ", " + sumAccY;
                        accCnt = 0;
                        sumAccX = 0;
                        sumAccY = 0;
                    }
                }
                    
            }

            oldAngles = angles;

        }

        if(reconnect){
            reconnect = false;
            StartCoroutine(sensorReconnect(1));

        }

        //Debug.Log("votes : " + votes[0] + " " + votes[1] + " " + votes[2] + " " + votes[3] + " : " + votesJ[0] + " "  + votesJ[1] + " " + votesJ[2] + " " + votesJ[3]);
        //Debug.Log(quatBuffer.Count); 
        
    }


    DateTime StartDateTime;
    DateTime epochStart;
    string timeString;
    string dateTime;
    int cur_time;
    String header;
    String filename;

    public void saveCSV()
    {

        epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        cur_time = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
        StartDateTime = DateTime.Now;
        dateTime = StartDateTime.ToString("dd-MM-yyyy");
        timeString = StartDateTime.ToString("hh-mm-ss");
        //myCSVFileManager = gameObject.AddComponent(typeof(CSVFileManager)) as CSVFileManager;
       // myCSVFileManager = new CSVFileManager();

        if(!string.IsNullOrEmpty(recordData)){
            header = "sensor number, min Sagittal plane, min Frontal plane, min Transverse plane, max Sagittal plane, max Frontal plane, max Transverse plane, min Sagittal speed, min Frontal speed, min Transverse speed, max Sagittal speed, max Frontal speed, max Transverse speed, min acceleration Frontal, min acceleration Longitudinal, min acceleration Sagittal, max acceleration Frontal, max acceleration Longitudinal, max acceleration Sagittal\n";
            filename = dateTime + "-" + timeString + "IMU_No" + Int32.Parse(sensorId, System.Globalization.NumberStyles.HexNumber).ToString() + "RecordData.csv";
           // myCSVFileManager.writeStringToFile(header + recordData, filename);
        }
        if(!string.IsNullOrEmpty(sensorData)){
            header = "sensor number, Sagittal plane, Frontal plane, Transverse plane, Sagittal speed, Frontal speed, Transverse speed, acceleration Frontal, acceleration Longitudinal, acceleration Sagittal\n";
            filename = dateTime + "-" + timeString + "IMU_No" + Int32.Parse(sensorId, System.Globalization.NumberStyles.HexNumber).ToString() + "FullData.csv";
           // myCSVFileManager.writeStringToFile(header + sensorData, filename);
        }

        if(!string.IsNullOrEmpty(avrData)){
            header = "sensor number, avg Sagittal plane, avg Frontal plane, avg Transverse plane, avg Sagittal speed, avg Frontal speed, avg Transverse speed, avg acceleration Frontal, avg acceleration Longitudinal, avg acceleration Sagittal\n";
            filename = dateTime + "-" + timeString + "IMU_No" + Int32.Parse(sensorId, System.Globalization.NumberStyles.HexNumber).ToString() + "AvrData.csv";
            //myCSVFileManager.writeStringToFile(header + avrData, filename);
        }

        if(!string.IsNullOrEmpty(quatData)){
            header = "sensor number, w, x, y, z\n";
            filename = dateTime + "-" + timeString + "IMU_No" + Int32.Parse(sensorId, System.Globalization.NumberStyles.HexNumber).ToString() + "QuatData.csv";
            //myCSVFileManager.writeStringToFile(header + quatData, filename);
        }

    }

    void OnDestroy(){

        Debug.Log("Application ending");
        saveCSV();
        if(socketConnection != null){
            socketConnection.GetStream().Close();
            socketConnection.Client.Close();
            socketConnection.Close();
        }
        if(clientReceiveThread != null){
            clientReceiveThread.Abort();
        }
        Resources.UnloadUnusedAssets();
        System.GC.Collect();

    }

}

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class EMGListener : MonoBehaviour
{
    public string sensorId = "00";
    string ip;
    int port;
    static readonly int framesize = 15;
    TcpClient socketConnection;
    NetworkStream stream;
    Thread clientReceiveThread;

    Boolean reconnect = false;
    float emg = 0;
    Queue<float> emgBuffer = new Queue<float>();
    int n = 0, btn = 0;
    //CSVFileManager myCSVFileManager;


    IEnumerator sensorReconnect(int secs){
        
        reconnect = false;
        emg = 0;
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

        ip = "172.24.60." + (230 - Int32.Parse(sensorId, System.Globalization.NumberStyles.HexNumber));
        port = 8000 + Int32.Parse(sensorId, System.Globalization.NumberStyles.HexNumber);
        clientReceiveThread = new Thread(new ThreadStart(ListenForData));
        clientReceiveThread.Start();

    }


    /// Runs in background clientReceiveThread; Listens for incomming data. 	
    private void ListenForData(){

        Byte[] bytes = new Byte[framesize];
        int length;

        try{ 
            socketConnection = new TcpClient(ip, port);
        }
        catch (SocketException socketException){
            Debug.Log("Socket exception: " + socketException);
            reconnect = true;
            return;
        }

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
                    emg = float.Parse(words[1]);
                    btn = int.Parse(words[2]);
                    //Debug.Log("datasent of 0x" + n + " : " + emg + " " + btn);
                    emgBuffer.Enqueue(emg);
                }
                catch (Exception e){
                    Debug.Log("{0} Exception caught." + e.ToString());
                }
            }
        }
        reconnect = true;
        return;

    }

    public String sensorData = "";
    public String avrData = "";
    public String recordData = "";
    float readyEmg;
    float rms;
    float avrEmg;
    float minEmg;
    float maxEmg;
    int cnt = 0;
    static readonly int avrFactor = 50;
    //TextWriter tw;

    // Update is called once per frame
    void Update() {
        if(emgBuffer.Count != 0){
            readyEmg = emgBuffer.Dequeue();
            //Debug.Log(readyEmg);
            readyEmg = (readyEmg - 1.665f) * 0.9910802775f;
            sensorData += sensorId + ", " + readyEmg + "\n";
            recordData = sensorId + ", " + minEmg + ", " + maxEmg + "\n";
            //Debug.Log(recordData);
            if(cnt > avrFactor){
                avrEmg /= avrFactor;
                rms = (float) Math.Sqrt(rms/avrFactor);
                Debug.Log("emg of 0x" + n + " : " + rms);
                avrData += sensorId + ", " + rms + ", " + avrEmg + "\n";
                cnt = 0;
                avrEmg = 0;
            }
            else{
                avrEmg += readyEmg;
                rms += readyEmg * readyEmg;
                minEmg = minEmg < readyEmg ? minEmg : readyEmg;
                maxEmg = maxEmg > readyEmg ? maxEmg : readyEmg;
                cnt++;
            }
        }

        if(reconnect){

            StartCoroutine(sensorReconnect(1));

        }
         
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
        //myCSVFileManager = new CSVFileManager();

        if(!string.IsNullOrEmpty(recordData)){
            header = "sensor number, min emg, max emg\n";
            filename = dateTime + "-" + timeString + "emg_No" + Int32.Parse(sensorId, System.Globalization.NumberStyles.HexNumber).ToString() + "RecordData.csv";
            //myCSVFileManager.writeStringToFile(header + recordData, filename);
        }
        if(!string.IsNullOrEmpty(sensorData)){
            header = "sensor number, emg\n";
            filename = dateTime + "-" + timeString + "emg_No" + Int32.Parse(sensorId, System.Globalization.NumberStyles.HexNumber).ToString() + "FullData.csv";
           // myCSVFileManager.writeStringToFile(header + sensorData, filename);
        }

        if(!string.IsNullOrEmpty(avrData)){
            header = "sensor number, rms, avg emg\n";
            filename = dateTime + "-" + timeString + "emg_No" + Int32.Parse(sensorId, System.Globalization.NumberStyles.HexNumber).ToString() + "AvrData.csv";
            //myCSVFileManager.writeStringToFile(header + avrData, filename);
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

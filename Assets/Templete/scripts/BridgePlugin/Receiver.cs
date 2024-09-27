using UnityEngine;
using UnityEngine.Events;

public class Receiver : MonoBehaviour
{
    //public UnityEvent<BridgeDataModel> OnReceiveEvent;// = new UnityEvent<BridgeDataModel>();
    [SerializeField] GameEvent OnRecieveCloseApp;
    [SerializeField] GameEvent OnRecieveStartApp;
    //[SerializeField] StringVariable actionType;//com.VRapeutic.CLOSE_APP
    [SerializeField] BoolValue generateCSVFile;
    int[] settings=new int[10];
    [SerializeField] StringVariable sesionId;


    public void OnReceiveCloseApp(string messageFromNative)
    {
        CloseAppClass closeAppInstant = JsonUtility.FromJson<CloseAppClass>(messageFromNative);
        OnRecieveCloseApp.Raise();
        //actionType.Value = closeAppInstant.action;
        generateCSVFile.Value = closeAppInstant.generateCsvReport;
        //Debug.Log($"Action is {bridgeDataModel.action} and GenerateCsvReport is {bridgeDataModel.generateCsvReport}");

    }

    public void OnReceiveStartIntent(string messageFromNative)
    {
        StartAppClass startAppInstant = JsonUtility.FromJson<StartAppClass>(messageFromNative);
        OnRecieveStartApp.Raise();
        settings = startAppInstant.settings;
        sesionId.Value = startAppInstant.sessionId;
        if (GetComponent<MappingChoices>() != null) GetComponent<MappingChoices>().Mapper(settings);
        //Debug.Log($"Action is {bridgeDataModel.action} and GenerateCsvReport is {bridgeDataModel.generateCsvReport}");
        for (int i = 0; i < 10; i++)
        {
            Debug.Log("settings ["+i+"]="+settings[i]);
        }
        Debug.Log("sesion id ="+ sesionId.Value);
    }
}

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Receiver : MonoBehaviour
{
    //public UnityEvent<BridgeDataModel> OnReceiveEvent;// = new UnityEvent<BridgeDataModel>();
    [SerializeField] GameEvent OnRecieveCloseApp;
    [SerializeField] GameEvent OnRecieveStartApp;
    //[SerializeField] StringVariable actionType;//com.VRapeutic.CLOSE_APP
    [SerializeField] BoolValue generateCSVFile;
    int[] settings=new int[10];
    [SerializeField] StringVariable sesionId;

    bool canEnterStartIntent = true;
    bool canEnterCloseIntent = true;


    public void OnReceiveCloseApp(string messageFromNative)
    {
        if (canEnterCloseIntent && (SceneManager.GetActiveScene().name != "SystemLobby"))
        {
            canEnterCloseIntent = false;
            CloseAppClass closeAppInstant = JsonUtility.FromJson<CloseAppClass>(messageFromNative);
            OnRecieveCloseApp.Raise();
            //actionType.Value = closeAppInstant.action;
            generateCSVFile.Value = closeAppInstant.generateCsvReport;
            //Debug.Log($"Action is {bridgeDataModel.action} and GenerateCsvReport is {bridgeDataModel.generateCsvReport}");
        }
    }

    public void OnReceiveStartIntent(string messageFromNative)
    {
        if (canEnterStartIntent && (SceneManager.GetActiveScene().name == "SystemLobby"))//SystemLobby
        {
            canEnterStartIntent = false;
            StartAppClass startAppInstant = JsonUtility.FromJson<StartAppClass>(messageFromNative);
            OnRecieveStartApp.Raise();
            settings = startAppInstant.settings;
            sesionId.Value = startAppInstant.sessionId;
            if (GetComponent<MappingChoices>() != null) GetComponent<MappingChoices>().Mapper(settings);
            //Debug.Log($"Action is {bridgeDataModel.action} and GenerateCsvReport is {bridgeDataModel.generateCsvReport}");
            for (int i = 0; i < 10; i++)
            {
                Debug.Log("settings [" + i + "]=" + settings[i]);
            }
            Debug.Log("sesion id =" + sesionId.Value);
        }
    }
}

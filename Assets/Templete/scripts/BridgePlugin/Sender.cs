using UnityEngine;
using UnityEngine.UI;

public class Sender : MonoBehaviour
{
    [SerializeField] private Button sendButton;

    private void Awake()
    {
        sendButton.onClick.AddListener(SendInputFromUserToNatvie);
    }
    private void SendInputFromUserToNatvie()
    {
        BridgePluginInitializer.Instance.SendIntent("path/to/csv_file");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using OldStats;

public class PutRequest : MonoBehaviour
{

    public static PutRequest instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        JsonAPIS.instance.PutRequest();
    }
}

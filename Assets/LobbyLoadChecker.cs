using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyLoadChecker : MonoBehaviour
{
    public static LobbyLoadChecker instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this)Destroy (this) ;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnDestroy()
    {
        Debug.Log("on destroy from lobby load checker");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using Tachyon;
using UnityEngine;

public class GamMangerTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InvokationManager invokationManager = new InvokationManager(this, this.gameObject.name);
        NetworkManager.InvokeClientMethod("NewTestRPC", invokationManager);
        //StartCoroutine(BeginLevelIenum());
        //BeginLevel();
    }


    IEnumerator BeginLevelIenum()
    {
        //Debug.Log("GameManager.BeginLevelIenum()");
        //yield return new WaitForSeconds(2);
        //NetworkManager.InvokeServerMethod("BeginLevelRPC", this.gameObject.name);

        yield return null;
        NetworkManager.InvokeServerMethod("NewTestRPC", this.gameObject.name);
    }

    public void BeginLevel()
    {
        NetworkManager.InvokeServerMethod("NewTestRPC", this.gameObject.name);
    }

    public void NewTestRPC()
    {
        Debug.Log("GamMangerTest.NewTestRPC()");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

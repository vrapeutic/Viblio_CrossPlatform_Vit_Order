using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDistracting : MonoBehaviour
{
    public async void OpenAndCloseTheDoor()
    {
        GetComponent<Animator>().SetTrigger("Open");
        GetComponent<AudioSource>().Play();
        await new WaitForSeconds(2.5f);
        GetComponent<Animator>().SetTrigger("Close");

    }
}

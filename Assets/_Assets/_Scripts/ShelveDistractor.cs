using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelveDistractor : MonoBehaviour
{
    public async void BreakTheShelf()
    {
        await new WaitForSeconds(.1f);
        GetComponent<Animator>().SetTrigger("Break");
    }
    public void FixTheShelf()
    {
        GetComponent<Animator>().SetTrigger("Fix");
    }
}

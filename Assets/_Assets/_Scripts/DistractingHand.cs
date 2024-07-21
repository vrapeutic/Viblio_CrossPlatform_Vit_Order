using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractingHand : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RightHand")
        {
            GameManager.instance.FireOnCompleteDistractingTask();
            StartCoroutine(ResetCollidierForWhile());
        }
        
    }
    IEnumerator ResetCollidierForWhile()
    {
        GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(15);
        GetComponent<BoxCollider>().enabled = true;
    }
}

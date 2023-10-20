using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractingHand : MonoBehaviour
{
    [SerializeField]GameEvent OnCompleteDistracting;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RightHand")
        {
            OnCompleteDistracting.Raise();
            StartCoroutine(ResetCollidierForWhile());
        }
        
    }
    IEnumerator ResetCollidierForWhile()
    {
        GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(32);
        GetComponent<BoxCollider>().enabled = true;
    }
}

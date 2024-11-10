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
            StartCoroutine(ResetCollidierForWhile());
            OnCompleteDistracting.Raise();
        }
        
    }
    IEnumerator ResetCollidierForWhile()
    {
        GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(5);
        GetComponent<BoxCollider>().enabled = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractingHandForShelf : MonoBehaviour
{
    [SerializeField] GameEvent OnCompleteDistracting;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RightHand")
        {
            OnCompleteDistracting.Raise();
            this.gameObject.SetActive(false);
        }
    }
}

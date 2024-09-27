using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSensor : MonoBehaviour
{
    [SerializeField] int sensNo;//1 for the right sensor 0 for the left one 

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("RightHand"))
        {
            transform.GetComponentInParent<WavingDetection>().DetectMove(sensNo);
        }
    }
}

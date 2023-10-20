using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenBookSensor : MonoBehaviour
{
    [SerializeField] IntVariable fallenBookNo;
    // Start is called before the first frame update
    void Start()
    {
        fallenBookNo.Value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PutBook")) fallenBookNo.Value++; 
    }
}

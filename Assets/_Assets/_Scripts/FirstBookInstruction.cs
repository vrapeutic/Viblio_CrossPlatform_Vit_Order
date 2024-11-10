using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBookInstruction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<DistractorInstructionController>().PlayInstruction();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObject : MonoBehaviour
{
    [SerializeField] GameObject disabledObject;

    //after compileted object distractor 

    public void DisableTheObjectAndRestAnim()
    {
        transform.GetComponent<Animator>().Play("idle", -1, 0f);
        disabledObject.SetActive(false);
    }
}

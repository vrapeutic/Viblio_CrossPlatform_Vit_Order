using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalEnabilg : MonoBehaviour
{
    [SerializeField] BoolValue condition;
    [SerializeField] bool reverseTheCondition;

    private void OnEnable()
    {
        if (reverseTheCondition)
        {
            if (condition.Value) gameObject.SetActive(false); ;
        }
        else
        {
            if (!condition.Value)gameObject.SetActive(false) ;
        }
    }
}

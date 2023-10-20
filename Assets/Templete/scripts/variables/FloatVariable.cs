using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class FloatVariable : ScriptableObject
{
    public float Value;

    public void SetValue(int value)
    {
        Value = value;
    }
    private void OnEnable()
    {
        hideFlags = HideFlags.DontUnloadUnusedAsset;
    }
}

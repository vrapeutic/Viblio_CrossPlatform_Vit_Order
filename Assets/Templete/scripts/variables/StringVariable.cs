using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StringVariable : ScriptableObject
{
    public string Value;

    private void OnEnable()
    {
        hideFlags = HideFlags.DontUnloadUnusedAsset;
    }

    public void SetValue(string value)
    {
        Value = value;
    }
}

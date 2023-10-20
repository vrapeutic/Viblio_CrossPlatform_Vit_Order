using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class IntVariable : ScriptableObject
{
    public int Value;

    public void SetValue(int value)
    {
        Value = value;
    }
    private void OnEnable()
    {
        hideFlags = HideFlags.DontUnloadUnusedAsset;
    }
}

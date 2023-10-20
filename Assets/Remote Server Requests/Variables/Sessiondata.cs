using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Sessiondata : ScriptableObject
{
    public int moduleId =-1;
    public int id =-1;
    public string headset ="";
    public string roomId ="";

    private void OnEnable()
    {
        hideFlags = HideFlags.DontUnloadUnusedAsset;
    }
}

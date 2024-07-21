using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Debbuger : MonoBehaviour
{
    Text tex;
    // Start is called before the first frame update
    void Start()
    {
        tex = GetComponent<Text>();
    }

    public void OnFirstHover()
    {
        tex.text += "OnFirstHover\n";
    }

    public void OnHoverEnter()
    {
        tex.text += "OnHoverEnter\n";
    }

    public void OnHoverExit()
    {
        tex.text += "OnHoverExit\n";
    }

    public void OnLastHoverExit()
    {
        tex.text += "OnLastHoverExit\n";
    }

    public void OnSelectEnter()
    {
        tex.text += "OnSelectEnter\n";
    }

    public void OnSelectExit()
    {
        tex.text += "OnHoverExit\n";
    }
}

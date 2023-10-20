using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCanvasSelector : MonoBehaviour
{
    [SerializeField]
    int languageInex;
    // Start is called before the first frame update
    void Start()
    {
        if (Statistics.instance.languageIndex != languageInex) Destroy(this.gameObject);
    }

}

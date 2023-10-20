using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCertainLevel : MonoBehaviour
{
    [SerializeField] int levelNo;
    // Start is called before the first frame update
    void Start()
    {
        if (Statistics.instance.level == levelNo)Destroy(this.gameObject) ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

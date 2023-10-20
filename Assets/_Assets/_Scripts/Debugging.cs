using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Debugging : MonoBehaviour
{

    Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        StartCoroutine(UpdateText());
    }

    // Update is called once per frame
    IEnumerator UpdateText()
    {
        while (true)
        {
            yield return new WaitForSeconds(.5f);
            text.text = Statistics.instance.correctPutBooksNo.ToString();
        }
    }
}

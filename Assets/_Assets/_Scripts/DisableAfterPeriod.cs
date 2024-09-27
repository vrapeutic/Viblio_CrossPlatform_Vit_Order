using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterPeriod : MonoBehaviour
{
    [SerializeField] float timeToWait;
    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(WaitAndDisable());
    }

     IEnumerator WaitAndDisable()
    {
        yield return new WaitForSeconds(timeToWait);
        gameObject.SetActive(false);
    } 
}

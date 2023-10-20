using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDisable : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(DisableThisGameobject());
    }

    IEnumerator DisableThisGameobject()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }
}

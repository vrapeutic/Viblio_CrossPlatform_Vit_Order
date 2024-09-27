using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelctiveDistractorFollowingTimeDetectors : MonoBehaviour
{
    [SerializeField] FloatVariable timeFollowingSelectiveDistractor;
    private void OnEnable()
    {
        timeFollowingSelectiveDistractor.Value = 0;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("SelectiveDistractor")) timeFollowingSelectiveDistractor.Value+=Time.deltaTime;
    }

}

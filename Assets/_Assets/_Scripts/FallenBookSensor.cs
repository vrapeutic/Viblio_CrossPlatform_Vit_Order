using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenBookSensor : MonoBehaviour
{
    [SerializeField] IntVariable fallenBookNo;
    // Start is called before the first frame update
    void OnEnable()
    {
        fallenBookNo.Value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PutBook")) { fallenBookNo.Value++;
            try
            {
                transform.parent.GetComponent< HideFallenShelfBooks>(). booksToHide.Add(other.gameObject);

            }
            catch (System.Exception)
            {
                Debug.Log("transform.parent.GetComponent< HideFallenShelfBooks> didn`t exist");
            }
        }
    }
}

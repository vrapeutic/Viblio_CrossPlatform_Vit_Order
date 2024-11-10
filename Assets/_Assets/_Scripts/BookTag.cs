using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookTag : MonoBehaviour
{
    [HideInInspector]
    public string bookTag;
    [HideInInspector]
    public string newName;
    public float localHightDiff = 0;
    bool canEndGrap= false; // to handle the case when book translated from book to another
    // Start is called before the first frame update
    void Start()
    {
        bookTag = gameObject.tag;
        //**//
        gameObject.tag = "Book";
        //StartCoroutine(ResetBookTag());
    }

    public void OnGrabEndFunc()//on last hover Exit
    {
        if (canEndGrap) 
        {
            //GetComponent<Rigidbody>().isKinematic = false;
            //GetComponent<Rigidbody>().useGravity = true;
            if (gameObject.tag == "GrabbedBook") gameObject.tag = "Book";
            //Debug.Log("OnGrabEndFunc");
        }
    }

    public void OnGrabBeginFunc()//on select
    {
        //GetComponent<Rigidbody>().isKinematic = true;
        //Debug.Log(gameObject.name + " OnGrabBeginFunc");
        gameObject.tag = "GrabbedBook";
        //Debug.Log("OnGrabBeginFunc");
        StartCoroutine(CanEndGrapIEnum());
    }

    IEnumerator CanEndGrapIEnum()
    {
        canEndGrap = false;
        yield return new WaitForSeconds(.1f);
        canEndGrap = true;

    }

    IEnumerator ResetBookTag()
    {
        gameObject.tag = "Book";
        yield return new WaitForSeconds(1f);
        gameObject.tag = bookTag ;
    }

}

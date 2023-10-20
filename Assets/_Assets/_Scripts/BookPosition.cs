using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class BookPosition : MonoBehaviour
{
    [SerializeField] string bookTag;//the tag that entered book must have
    [SerializeField] bool reservedPos;//position will not be available if true
    static int num = 0;
    bool positionOccupied = false;
    bool bookEntered = false;
    WaitForSeconds a5PartSeconds;
    WaitForSeconds a2PartSeconds;
    WaitForSeconds a3PartSeconds;
    WaitForSeconds a2Seconds;
    WaitForSeconds a1second;
    int lastTarget = -1;
    NPCSoundController speaker;
    //static int correctPutBook=0;
    static bool CanResetCorrectButvalue=true;
    bool CanRestCollidiersForInstance = true;
    bool secondCheck = false;


    private void Start()
    {
        a2Seconds = new WaitForSeconds(2);
        a1second = new WaitForSeconds(1);
        a5PartSeconds = new WaitForSeconds(.5f);
        a2PartSeconds = new WaitForSeconds(.2f);
        a3PartSeconds = new WaitForSeconds(.3f);
        speaker = FindObjectOfType<NPCSoundController>();
        OnStart();
    }

    private  void OnStart()
    {
        if (reservedPos)
        {
            ControlIndictors(0, false);
            StartWithInstaniateBooks();
        }
        else
        {
            ControlIndictors(2, false);
            //for (int i = 3; i < 7; i++)
            //{
            //    transform.GetChild(i).transform.gameObject.SetActive(false);
            //}
        }
        if (!Statistics.instance.android) return;

        RestCollidiersForInstance();
    }


    //we can grab the ready books on the shelf
    public void StartWithInstaniateBooks()//int _num)
    {
        if (bookTag == "BookYellow")Instantiate(Resources.Load("Books/Yellow Book"), transform.position, Quaternion.identity, transform).name = ("B" + gameObject.name);
        else if (bookTag == "BookBlue") Instantiate(Resources.Load("Books/Blue Book"), transform.position, Quaternion.identity, transform).name = ("B" + gameObject.name);
        else Instantiate(Resources.Load("Books/Red Book"), transform.position, Quaternion.identity, transform).name = ("B" + gameObject.name);
        if (Statistics.instance.android) return;
        transform.GetComponentInChildren<Rigidbody>().isKinematic = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Book")
        {
            bookEntered = true;
            if (!positionOccupied)
            {

                other.gameObject.tag = "PutBook";//if user release the book horizontally
                positionOccupied = true;
 /**/ //               if (other.gameObject.GetComponent<OVRGrabbable>().grabbedBy != null) other.gameObject.GetComponent<OVRGrabbable>().grabbedBy.ForceRelease(other.gameObject.GetComponent<OVRGrabbable>());
                other.gameObject.transform.position = transform.position;
                other.gameObject.transform.rotation = transform.rotation;
                other.GetComponent<Rigidbody>().isKinematic = true;
                other.gameObject.GetComponent<BookTag>().newName = gameObject.name;

                if (other.GetComponent<BookTag>().bookTag != bookTag)
                {
                    Statistics.instance.wrongTrials++;
                    Statistics.instance.totalTrials++;
                    //Debug.Log("TAR : Total no of trials : " + Statistics.instance.totalTrials);
                }
                if (GameManager.instance.currentlyPlaying) Statistics.instance.StartResponseTimer();
            }
        }
        else if (other.tag == "PutBook")
        {
            bookEntered = true;
            if (other.gameObject.GetComponent<BookTag>().newName == gameObject.name)//my book puted 
            {
                if (other.GetComponent<BookTag>().bookTag == bookTag)//Right attempt
                {
                    if (secondCheck)
                    {
                        Statistics.instance.correctPutBooksNo++;
                        Statistics.instance.totalTrials++;
 /**/                   other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                        try
                        {
                            if (other.gameObject.GetComponent<XRGrabInteractable>() != null && other.gameObject.GetComponent<XRGrabInteractable>().enabled)
                            {
                                other.gameObject.GetComponent<XRGrabInteractable>().enabled = false;
                                Books.instatnce.bookshights.Add(other.gameObject.transform.position.y);
                            }
                        }
                        catch { }//Debug.Log("TAR : books placed correctly : "+ Statistics.instance.correctPutBooksNo);
                        //Debug.Log("TAR : Total no of trials : " + Statistics.instance.totalTrials);
                    }
                    ControlIndictors(0, true);
                }
                else //Wrong attempt
                {
                    ControlIndictors(1, true);
                }
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<BookTag>() == null) return;
        if (other.gameObject.GetComponent<BookTag>().newName == gameObject.name)
        {
            BookExit();
        }
    }

    private void BookExit()
    {
        positionOccupied = false;
        ControlIndictors(2, true);
    }

    //2 empty pos //0 correct pos //1 wrongpos
    //two copies from this function because I want visual reward not to delay because of server letancy
    void ControlIndictors(int target, bool server)
    {
        if (lastTarget == target) return;
        for (int i = 0; i < 3; i++)
        {
            if (i == target) transform.GetChild(i).transform.gameObject.SetActive(true);
            else transform.GetChild(i).transform.gameObject.SetActive(false);
        }
        if (server) ControlRewardsRPC(target,Statistics.instance.correctPutBooksNo,Random.Range(0,2));
        lastTarget = target;
    }
    public void ControlRewardsRPC(int target, int booksNo, int clipNo)
    {
        if (target == 0&&(Statistics.instance.level!=2))
        {
            if (booksNo % 2 == 1) speaker.PlayInstructions(clipNo);
        }
        else if (target == 1)
            speaker.PlayInstructions(2);
        if (Statistics.instance.android) return;
        for (int i = 0; i < 3; i++)
        {
            if (i == target) transform.GetChild(i).transform.gameObject.SetActive(true);
            else transform.GetChild(i).transform.gameObject.SetActive(false);
        }
    }

    public void RestCollidiersForInstance()
    {
        if (!Statistics.instance.android) return;
        if (!CanRestCollidiersForInstance) return;
        CanRestCollidiersForInstance = false;
        StartCoroutine(RestCanRestCollidiersForInstance());
        RestCollidiersForInstanceTwice();
    }

    IEnumerator RestCanRestCollidiersForInstance()
    {
        yield return new WaitForSeconds(1.5f);
        CanRestCollidiersForInstance = true;
    }

    async void RestCollidiersForInstanceTwice()
    {
        StartCoroutine(RestCollidier(a2PartSeconds));
        secondCheck = false;
        await a3PartSeconds;
        if (CanResetCorrectButvalue)
        {
            CanResetCorrectButvalue = false;
            Statistics.instance.correctPutBooksNo = 0;
        }
        secondCheck = true;
        bookEntered = false;
        StartCoroutine(RestCollidier(a2PartSeconds));
        CanResetCorrectButvalue = true;
        await a5PartSeconds;
        CheckEmpty();
    }

    IEnumerator RestCollidier(WaitForSeconds timeToWait)
    {
        GetComponent<Collider>().enabled = false;
        yield return timeToWait;
        GetComponent<Collider>().enabled = true;
    }

    void CheckEmpty()//this call after 1s from release any book
    {
        if (!bookEntered )
        {
            BookExit();
        }
    }
}
/*
Depricated code area
void StartWithReservedPositions()
{
    GetComponent<BoxCollider>().isTrigger = false;
    transform.GetChild(0).transform.gameObject.SetActive(true);//green
    transform.GetChild(0).transform.GetChild(0).transform.gameObject.SetActive(false);//face Emojii
    transform.GetChild(1).transform.gameObject.SetActive(false);//red
    transform.GetChild(2).transform.gameObject.SetActive(false);//white
    //show book parts
    for (int i = 3; i < 7; i++)
    {
        transform.GetChild(i).transform.gameObject.SetActive(true);
    }
}
*/
//we can`t grab the ready books on the shelf

//#region assurance
//private void OnTriggerStay(Collider other)
//{
//    if (other.gameObject.tag == "PutBook"&& other.gameObject.GetComponent<BookTag>().newName == gameObject.name)
//    {
//        if (other.GetComponent<BookTag>().bookTag == bookTag)
//            correctBookStayed = true;
//        wrongBookStayed = true;
//    }
//}

////async void CheckBookStayed()
////{
////    await CheckBookStayedIenum();
////}

//private IEnumerator CheckBookStayedIenum()
//{
//    canCheck = true;
//    while (canCheck)
//    {
//        correctBookStayed = false;
//        wrongBookStayed = false;
//        yield return new WaitForSeconds(2);
//        if ((!correctBookStayed)&&positionOccupied)
//        {
//            GameManager.instance.DecreaseCorrectPutBook();
//            BookExit();
//            canCheck = false;
//        }
//        else if ((!wrongBookStayed) && positionOccupied)
//        {
//            BookExit();
//            canCheck = false;
//        }
//        Debug.Log(gameObject.name + correctBookStayed + " " + wrongBookStayed);
//    }
//}
//#endregion

//else if(other.tag == "GrabbedBook"|| other.tag == "Environment" || other.tag == "Hand" )
//{

//}
//else //you must handle every thing to reach nothing
//{
//    if (correctBookLastTime == true)
//        GameManager.instance.DecreaseCorrectPutBook();
//    correctBookLastTime = false;
//    BookExit();
//}
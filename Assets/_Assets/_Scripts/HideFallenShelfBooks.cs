using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideFallenShelfBooks : MonoBehaviour
{
    public List<GameObject> booksToHide = new List<GameObject>();

    public void HiddingFalllenBooks()
    {
        StartCoroutine(HiddingFalllenBooksIenum());
    }

    IEnumerator HiddingFalllenBooksIenum()
    {
        yield return new WaitForSeconds(.2f);
        try
        {
            foreach (var item in booksToHide)
            {
                item.SetActive(false);
            }
        }
        catch (System.Exception)
        {
            Debug.Log("can`t find booksToHide");
        }
    }

    public void ShowingFalllenBooks()
    {
        try
        {
            foreach (var item in booksToHide)
            {
                item.SetActive(true);
            }
            booksToHide.Clear();
        }
        catch (System.Exception)
        {
            Debug.Log("can`t find booksToHide");
        }
    }

}

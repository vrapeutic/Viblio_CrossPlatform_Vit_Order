using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightPlus;

public class HighLightBooks : MonoBehaviour
{
    GameObject[] books;
    [SerializeField] HighlightProfile highlightProfile;
    [SerializeField] AudioClip ring
        ;
    // Start is called before the first frame update
    void Start()
    {
        books = new GameObject[5];
        for (int i = 0; i < books.Length; i++)
        {
            books[i] = transform.GetChild(i).gameObject;
        }

    }

    public  void MakeHighLightBooks()
    {
        for (int i = 0; i < books.Length; i++)
        {
            books[i].AddComponent<HighlightEffect>();
            books[i].GetComponent<HighlightEffect>().profile = highlightProfile;
            books[i].GetComponent<HighlightEffect>().profile.Load(books[i].GetComponent<HighlightEffect>());
            books[i].AddComponent<AudioSource>();
            books[i].GetComponent<AudioSource>().clip = ring;
            books[i].GetComponent<AudioSource>().playOnAwake = false;
            books[i].GetComponent<AudioSource>().volume = .5f;

        }
        StartCoroutine( PlayTheBookSequence());
    }
    IEnumerator PlayTheBookSequence()
    {
        for (int i = 0; i < books.Length; i++)
        {
            if (i % 3 == 0) yield return new WaitForSeconds(1.4f);
            else yield return new WaitForSeconds(.7f);
            books[i].GetComponent<AudioSource>().Play();
            books[i].GetComponent<HighlightEffect>().highlighted = true;
            if (i != 0)
            {
                Destroy(books[i - 1].GetComponent<AudioSource>());
                Destroy(books[i - 1].GetComponent<HighlightEffect>());
            }
        }
        yield return new WaitForSeconds(1.4f);
        Destroy(books[9].GetComponent<AudioSource>());
        Destroy(books[9].GetComponent<HighlightEffect>());
    }
}

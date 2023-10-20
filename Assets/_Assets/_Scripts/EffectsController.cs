using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsController : MonoBehaviour
{
    [SerializeField] GameObject level1IntroParticles;
    [SerializeField] GameObject darkSphere;
    [SerializeField] GameObject FlashBackSphere;
    [SerializeField] AudioSource booksFallSound;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void StartLevel1IntroEffect()
    {
        //RenderSettings.fog = true;
        FlashBackSphere.SetActive(true);
        level1IntroParticles.SetActive(true);
    }

    public void StopLevel1IntroEffect()
    {
        //RenderSettings.fog = false;
        FlashBackSphere.SetActive(false);
        level1IntroParticles.SetActive(false);
    }

    public void StartFadeToDark ()
    {
        darkSphere.SetActive(true);
        //sounds
        booksFallSound.Play();
        StartCoroutine(StopFallingBooksSound());
    }

    IEnumerator StopFallingBooksSound()
    {
        yield return new WaitForSeconds(2f);
        booksFallSound.Stop();
    }

    public void StopFadeToDark()
    {
        darkSphere.SetActive(false);
    }


}

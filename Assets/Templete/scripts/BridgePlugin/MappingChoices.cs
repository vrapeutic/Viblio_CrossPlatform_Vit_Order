using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MappingChoices : MonoBehaviour
{
    [SerializeField] StringVariable typeOfAttention;//sustained //selective //adaptive
    //choose environment
    [SerializeField] IntVariable sustainedValue;
    [SerializeField] IntVariable noOfDistractor;

    public void Mapper(int[] settings)
    {
        if (settings[0] == 1) typeOfAttention.Value= "sustained";
        else if (settings[0] == 2) typeOfAttention.Value = "selective";
        else typeOfAttention.Value = "adaptive";

        if (settings[2] == 1) sustainedValue.Value = 20;
        else if (settings[2] == 2) sustainedValue.Value = 40;
        else sustainedValue.Value = 60;

        if (settings[3] == 1) noOfDistractor.Value = 1;
        else if (settings[3] == 2) noOfDistractor.Value = 2;
        else noOfDistractor.Value = 3;
        StartCoroutine(LoadGameScene());
    }

    public IEnumerator LoadGameScene()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1);
    }
}

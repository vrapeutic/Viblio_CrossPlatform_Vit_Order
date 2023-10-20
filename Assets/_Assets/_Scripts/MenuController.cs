using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    //[SerializeField] IntVariable noOfBooks;
    public void Setlevel(int level)
    {
        Statistics.instance.level = level;
    }

    public void SetType(bool isClosedTime)
    {
        Statistics.instance.isClosedTime = isClosedTime;
    }

    //public void SetNoOfBooks(int _noOfBooks)
    //{
    //    noOfBooks.Value = _noOfBooks;
    //}

    public void SetClosedTimeValue(int closedTimeValue)
    {
        Statistics.instance.closedTimeValue = closedTimeValue;
    }

    public void LoadScene()
    {
        //if (Statistics.instance.level == 1) SceneManager.LoadScene("Level1");
        //else if (Statistics.instance.level == 2) SceneManager.LoadScene("Level2");
        //else
         SceneManager.LoadScene("Game");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadSceneAsync("Main");
    }

    public void LoadSessionScene()
    {
        SceneManager.LoadSceneAsync("Session");
    }

    public void SkipIntroLevel()
    {
        FindObjectOfType<LevelManager>().GetComponent<ILevel>().SkipIntroLevel();
    }


    public void NextLevel()
    {
        StartCoroutine(NextLevelIenum());
    }

    IEnumerator NextLevelIenum()
    {
        Setlevel( ++Statistics.instance.level);
        yield return new WaitForSeconds(.5f);
        LoadScene();
    }

    //for on play canvas Exit when Exit while Playing
    public void EndScene()
    {
        GameManager.instance.EndUnSuccessfully();
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ActivateControllersRays()
    {
        Statistics.instance.isWithRay = true;
    }

}

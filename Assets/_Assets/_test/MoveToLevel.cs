using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToLevel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        //StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(4);
        LoadSceneRPC();
    }

    public void LoadSceneRPC()
    {
        if (Statistics.instance.level == 1) SceneManager.LoadScene("Level1");
        else if (Statistics.instance.level == 2) SceneManager.LoadScene("Level2");
        else SceneManager.LoadScene("Level3");
    }
}

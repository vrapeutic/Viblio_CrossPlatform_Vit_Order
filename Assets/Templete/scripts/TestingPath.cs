using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestingPath : MonoBehaviour
{
    [SerializeField] BoolValue testingMode;
    // Start is called before the first frame update
    void Start()
    {
        if(testingMode.Value) StartCoroutine(LoadMainSceneInum());
    }

    private IEnumerator LoadMainSceneInum()
    {
        yield return null;
        SceneManager.LoadSceneAsync(2);
    }
}
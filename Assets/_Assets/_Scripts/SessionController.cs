using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SessionController : MonoBehaviour
{
    [SerializeField] GameObject canvas;
    public bool canLoadSessionCanvas=false;
    public static SessionController instance;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //Debug.Log("the else!");
            Destroy(this.gameObject);
        }
        CheckAndShowCanvas();
    }

    public void LoadSessionScene()
    {
        BackendSession.instance.currentRoomId = "";
        BackendSession.instance.patient_id = "-1";
        SceneManager.LoadSceneAsync("Session");
    }

    private void OnDestroy()
    {
        instance.CheckAndShowCanvas();
    }

    public void CheckAndShowCanvas()
    {
        if (!canLoadSessionCanvas) canvas.SetActive(false);
        else
        {
            canvas.SetActive(true);
            canLoadSessionCanvas = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Tachyon;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InvokationManager invokationManager = new InvokationManager(this, this.gameObject.name);
        NetworkManager.InvokeClientMethod("SetlevelRPC", invokationManager);
        NetworkManager.InvokeClientMethod("LoadSceneRPC", invokationManager);
        NetworkManager.InvokeClientMethod("LoadMainMenuRpc", invokationManager);
        NetworkManager.InvokeClientMethod("ExitModuleRPC", invokationManager);
        NetworkManager.InvokeClientMethod("SetTypeRPC", invokationManager);
        NetworkManager.InvokeClientMethod("SetClosedTimeValueRPC", invokationManager);
        NetworkManager.InvokeClientMethod("LoadMainMenuRPC", invokationManager);
        NetworkManager.InvokeClientMethod("SkipIntroLevelRPC", invokationManager);
        //StartCoroutine(LoadLevel());
    }

    public void Setlevel(int level)
    {
        NetworkManager.InvokeServerMethod("SetlevelRPC", this.gameObject.name, level);
    }
    public void SetlevelRPC(int level)
    {
        Statistics.instance.level = level;
        Debug.Log("MenuController.SetlevelRPC()");
    }

    public void SetType(bool isClosedTime)
    {
        NetworkManager.InvokeServerMethod("SetTypeRPC", this.gameObject.name, isClosedTime);
    }
    public void SetTypeRPC(bool isClosedTime)
    {
        Statistics.instance.isClosedTime = isClosedTime;
    }

    public void SetClosedTimeValue(int closedTimeValue)
    {
        NetworkManager.InvokeServerMethod("SetClosedTimeValueRPC", this.gameObject.name, closedTimeValue);
    }
    public void SetClosedTimeValueRPC(int closedTimeValue)
    {
        Statistics.instance.closedTimeValue = closedTimeValue;
    }

    public void LoadScene()
    {
            NetworkManager.InvokeServerMethod("LoadSceneRPC", this.gameObject.name);       
    }
    public void LoadSceneRPC()
    {
        //Debug.Log("MenuController.LoadSceneRPC()");
        if (Statistics.instance.level==1) SceneManager.LoadScene("Level1");
        else if (Statistics.instance.level == 2) SceneManager.LoadScene("Level2");
        else SceneManager.LoadScene("Level3");
    }

    public void LoadMainMenu()
    {
        NetworkManager.InvokeServerMethod("LoadMainMenuRPC", this.gameObject.name);        
    }
    public void LoadMainMenuRPC()
    {
        //Debug.Log("android:" + Statistics.instance.android.ToString() + this.ToString() + " LoadMainMenuRPC()");
        SceneManager.LoadSceneAsync("Main");
    }

    public void SkipIntroLevel()
    {
        NetworkManager.InvokeServerMethod("SkipIntroLevelRPC", this.gameObject.name);
        //Debug.Log("SkipIntroLevel "+">MenuController >"+gameObject.name);
    }

    public void SkipIntroLevelRPC()
    {
        //Debug.Log("SkipIntroLevelRPC");
        FindObjectOfType<LevelManager>().GetComponent<ILevel>().SkipIntroLevel();
    }

    public void NextLevel()
    {
        StartCoroutine(NextLevelIenum());
    }

    IEnumerator NextLevelIenum()
    {
        NetworkManager.InvokeServerMethod("SetlevelRPC", this.gameObject.name, ++Statistics.instance.level);
        yield return new WaitForSeconds(.5f);
        NetworkManager.InvokeServerMethod("LoadSceneRPC", this.gameObject.name);
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

    private void OnDestroy()
    {
        InvokationManager invokationManager = new InvokationManager(this, this.gameObject.name);
        NetworkManager.RevokeClientMethod("SetlevelRPC", invokationManager);
        NetworkManager.RevokeClientMethod("LoadSceneRPC", invokationManager);
        NetworkManager.RevokeClientMethod("LoadMainMenuRpc", invokationManager);
        NetworkManager.RevokeClientMethod("ExitModuleRPC", invokationManager);
        NetworkManager.RevokeClientMethod("SetTypeRPC", invokationManager);
        NetworkManager.RevokeClientMethod("SetClosedTimeValueRPC", invokationManager);
        NetworkManager.RevokeClientMethod("LoadMainMenuRPC", invokationManager);
        NetworkManager.RevokeClientMethod("SkipIntroLevelRPC", invokationManager);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class KeybadInput : MonoBehaviour
{
    public string value = "";
    [SerializeField]Text vaueTxt;
    bool canInteract = true;
    [SerializeField] StringVariable sessionID;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AddKey(string key)
    {
        if (!canInteract) return;
        if (value.Length >= 20) return;
        value = value + key;
        vaueTxt.text = value;
    }

    public void RemoveKey()
    {
        if (!canInteract) return;
        value = value.Remove(value.Length-1);
        vaueTxt.text = value;
    }

    public void OnOkPressed()
    {
        if (!canInteract) return;
        canInteract = false;
        Debug.Log("OnOkPressed");
        sessionID.Value = value;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public void ResetCanInteract()
    {
        canInteract = true;
    }

    public void ResetValue()
    {
        value = "";
        vaueTxt.text = value;
    }
}

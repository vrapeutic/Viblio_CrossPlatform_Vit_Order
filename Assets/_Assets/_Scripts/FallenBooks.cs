using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenBooks : MonoBehaviour
{
    [SerializeField] IntVariable fallenBooksNo;

    private void Start()
    {
        InitFallenBooks();
    }
    async void InitFallenBooks()
    {
        SaveChildsIntialPos();
        await new WaitForSeconds(.5f);
        SettingChildsActive(false);
    }

    public async void ActivateFallenBooks()
    {
        await new WaitForSeconds(.1f);
        SettingChildsActive(false);
        SettingChildsActive(true);
    }

    public void DeactivateFallenBooks()
    {
        SettingChildsActive(false);
    }

    void SettingChildsActive(bool state )
    {
        for (int i = 0; i < fallenBooksNo.Value; i++)
        {
            transform.GetChild(i).gameObject.SetActive(state);
        }
    }

    void SaveChildsIntialPos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponentInChildren<FallenBookIntialPos>().SavePos();
        }
    }

}

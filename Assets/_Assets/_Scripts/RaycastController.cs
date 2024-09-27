using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 this script to controle which controller to play with      
     */
public class RaycastController : MonoBehaviour
{
    [SerializeField] GameObject [] rayControllers;
    [SerializeField] GameObject[] noRayControllers;
    // Start is called before the first frame update
    void Start()
    {
        ShowControllersWithRays();
    }

    private void OnEnable()
    {
        GameManager.OnLevelBegin += OnLevelStart;
    }

    private void OnDisable()
    {
        GameManager.OnLevelBegin -= OnLevelStart;
    }

    private void OnLevelStart()
    {
        if (Statistics.instance.isWithRay) ShowControllersWithRays();
        else HideControllersWithRays();
    }

    public void ShowControllersWithRays()
    {
        for (int i = 0; i < rayControllers.Length; i++)
        {
            rayControllers[i].SetActive(true);
            noRayControllers[i].SetActive(false);
        }
    }

    public void HideControllersWithRays()
    {
        for (int i = 0; i < rayControllers.Length; i++)
        {
            rayControllers[i].SetActive(false);
            noRayControllers[i].SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class XRButtonsHandler : MonoBehaviour
{
    [SerializeField] double longPressThreeshold;
    [SerializeField] GameEvent YButtonLongPress;
    [SerializeField] GameEvent BButtonLongPress;
    public InputActionReference pressYButton;
    public InputActionReference pressBButton;
    System.DateTime yButtonPressTime;
    System.DateTime bButtonPressTime;

    private void OnEnable()
    {
        pressYButton.action.started += SaveYButtonPressTime;
        pressBButton.action.started += SaveBButtonPressTime;
        pressYButton.action.canceled += CalculateYButtonLongPress;
        pressBButton.action.canceled += CalculateBButtonLongPress;

    }

    private void OnDisable()
    {
        pressYButton.action.started -= SaveYButtonPressTime;
        pressBButton.action.started -= SaveBButtonPressTime;
        pressYButton.action.canceled -= CalculateYButtonLongPress;
        pressBButton.action.canceled -= CalculateBButtonLongPress;
    }

    void SaveYButtonPressTime(InputAction.CallbackContext context)
    {
        yButtonPressTime = System.DateTime.Now;
    }

    void SaveBButtonPressTime(InputAction.CallbackContext context)
    {
        bButtonPressTime = System.DateTime.Now;
    }

    void CalculateYButtonLongPress(InputAction.CallbackContext context)
    {
        if((System.DateTime.Now - yButtonPressTime).TotalSeconds> longPressThreeshold)YButtonLongPress.Raise();
    }

    void CalculateBButtonLongPress(InputAction.CallbackContext context)
    {
        if ((System.DateTime.Now - bButtonPressTime).TotalSeconds > longPressThreeshold) BButtonLongPress.Raise();
    }
}

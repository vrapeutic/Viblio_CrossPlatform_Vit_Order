using Tachyon;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandHider : MonoBehaviour
{
    public GameObject handObject = null;

    private HandPhysics handPhysics = null;
    private XRBaseControllerInteractor interactor = null;
    public bool isShown = true;

    private void Start()
    {
        InvokationManager invokationManager = new InvokationManager(this, this.gameObject.name);
        NetworkManager.InvokeClientMethod("ShowRPC", invokationManager);
        NetworkManager.InvokeClientMethod("HideRPC", invokationManager);
    }

    private void Awake()
    {
        handPhysics = handObject.GetComponent<HandPhysics>();
        if (GetComponent<XRDirectInteractor>() == null)
            interactor = GetComponent<XRRayInteractor>();
        else
            interactor = GetComponent<XRDirectInteractor>(); 

    }

    private void OnEnable()
    {
        interactor.onSelectEntered.AddListener(Hide);
        interactor.onSelectExited.AddListener(Show);
    }

    private void OnDisable()
    {
        interactor.onSelectEntered.RemoveListener(Hide);
        interactor.onSelectExited.RemoveListener(Show);
    }

    private void Show(XRBaseInteractable interactable)
    {
        ShowHand();
    }

    public void ShowHand()
    {
        //handPhysics.TeleportToTarget();
        handObject.SetActive(true);
        isShown = true;
        //NetworkManager.InvokeServerMethod("ShowRPC", this.gameObject.name);
    }

    public void ShowHand(bool IsChangedState)
    {
        handObject.SetActive(true);
        if (IsChangedState) isShown = true; ;
    }

    public void HideHand(bool IsChangedState)
    {
        handObject.SetActive(false);
        if (IsChangedState) isShown = true; ;
    }

    public void ShowRPC()
    {
        if(!Statistics.instance.android) handObject.SetActive(true);
    }

    private void Hide(XRBaseInteractable interactable)
    {
        HideHand();
    }

    public void HideHand()
    {
        isShown = false;
        handObject.SetActive(false);
        NetworkManager.InvokeServerMethod("HideRPC", this.gameObject.name);
    }

    public void HideRPC()
    {
        if (!Statistics.instance.android) {
            handObject.SetActive(false);
            Debug.Log("Hide Hand RPC");
        }
    }

}

using UnityEngine;

public class BubbleUI : MonoBehaviour
{
    [SerializeField] private GameObject containerE;
    [SerializeField] private InteractCheck RangeCheck;
    private void Show()
    {
        containerE.SetActive(true);

    }
    private void Hide()
    {
        containerE.SetActive(false);
    }
    private void Update()
    {
        interactBtnCheck();
        if(gameMngt.Instance.gotKey) EnterDoor();
      
    }

    void interactBtnCheck()
    {
        if (RangeCheck.GetInteractableBox() != null) Show();
        else Hide();
    }
    void EnterDoor()
    {
        if (RangeCheck.CheckDoor())
        {
            Show();
        }
        else Hide();
    }
}

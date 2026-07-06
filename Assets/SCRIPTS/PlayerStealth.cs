using UnityEngine;

public class PlayerStealth : MonoBehaviour
{
    public delegate void StealthStateHandler(bool isStealth);
    public StealthStateHandler OnStealthChanged;
    public GameObject disguiseObj;
    private GameObject disguiseInstance;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            EnterStealth();
            if (Input.GetKeyDown(KeyCode.C)) ExitStealth();
        }
    }
    void EnterStealth()
    {
        disguiseInstance = Instantiate(disguiseObj,transform.position,Quaternion.identity);
        OnStealthChanged?.Invoke(true); //send event start stealth
    }
    void ExitStealth()
    {
        Destroy(disguiseInstance);
        OnStealthChanged?.Invoke(false); //sned event back to normal
    }
}

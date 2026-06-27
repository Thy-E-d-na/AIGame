using UnityEngine;

public class BoxScript : MonoBehaviour, I_Interactable
{
    [SerializeField] private GameObject vfx;
    public void Interact()
    {
        var v = Instantiate(vfx);
        Destroy(v, 1f);
        Destroy(gameObject);
    }
}

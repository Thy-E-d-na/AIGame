using UnityEngine;

public class BoxScript : MonoBehaviour, I_Interactable
{
    [SerializeField] private GameObject vfx;
    public void Interact()
    {
        soundMngt.sInstance.PlaySfx(0);
        var v = Instantiate(vfx, transform.position,Quaternion.identity);
        Destroy(v, 1f);
        Destroy(gameObject);
    }
}

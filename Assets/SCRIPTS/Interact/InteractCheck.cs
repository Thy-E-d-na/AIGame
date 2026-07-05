using UnityEngine;

public class InteractCheck : MonoBehaviour
{
    [SerializeField] private float InteractRange = 1f;
    [SerializeField] private float keyRange = 3f;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (GetInteractableBox() != null) GetInteractableBox().Interact();
            if (GetInteractableKey() != null) GetInteractableKey().Interact();
        }
    }


    public BoxScript GetInteractableBox()
    {
        Collider[] arr = Physics.OverlapSphere(transform.position, InteractRange);
        foreach (Collider col in arr)
        {
            if (col.TryGetComponent(out BoxScript interact))
            {

                return interact;
            }
        }
        return null;
    }
    public keyScript GetInteractableKey()
    {
        Collider[] arr = Physics.OverlapSphere(transform.position, keyRange);
        foreach (Collider col in arr)
        {
            if (col.TryGetComponent(out keyScript interact))
            {

                return interact;
            }
        }
        return null;
    }
}

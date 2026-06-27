using UnityEngine;

public class keyScript : MonoBehaviour, I_Interactable
{
    [SerializeField] private float rotSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var pos = transform.position;
        pos.y += 1;
        transform.position = pos;
    }
    public void Interact()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
    }
}

using UnityEngine;

public class keyScript : MonoBehaviour
{ 
    [SerializeField] private float rotSpeed;
    [SerializeField] private float radius;
    [SerializeField] private GameObject vfx;

    public GameObject takeUIPnl;
    bool canTake = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var pos = transform.position;
        pos.y += 1;
        transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
        var find = Physics.OverlapSphere(transform.position, radius);
        foreach (var p in find)
        {
            if (p.CompareTag("Player"))
            {
                takeToggle();
                break;
            }
            else
            {
                takeUIPnl.SetActive(false);
                canTake = false;
            }
        }
        if (canTake && Input.GetKeyDown(KeyCode.E))
        {
            gameMngt.Instance.gotKey = true;
            Destroy(takeUIPnl);
            var v = Instantiate(vfx, transform.position, Quaternion.identity);
            Destroy(v, 1f);
            Destroy(gameObject);
        }

    }
    void takeToggle()
    {
        takeUIPnl.SetActive(true);
        canTake = true;
    }


    
}

using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    const string IDLE = "Idle";
    const string RUN = "Run";
    const string STEALTH = "Stealth";

    //CLICK TO MOVE:
    private NavMeshAgent agent;
    private Animator anim;

    [Header("Movement")]
    public float walkSpeed = 3.5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float sampleDistance = 0.5f;
    [SerializeField] private GameObject clickEffect;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

    }

    public static event System.Action<Vector3> OnGroundTouch;
    private void Start()
    {
        walkSpeed = agent.speed;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit,200f, groundLayer))
            {
                //check if the clicked point is on the NavMesh
                if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, sampleDistance, NavMesh.AllAreas))
                {
                    if(clickEffect != null)
                    {
                        var click = Instantiate(clickEffect, hit.point += new Vector3(0,0.1f,0), Quaternion.identity);
                        Destroy(click, 1f);
                    }
                    agent.SetDestination(navHit.position);
                    
                    OnGroundTouch?.Invoke(navHit.position);
                }
                
            }
            else
                Debug.Log("Clicked point is not on the NavMesh.");
        }
        SetAnim();

    }

    void SetAnim()
    {
        if (agent.velocity == Vector3.zero)
        {
            anim.Play(IDLE);
        }
        else
        {
            anim.Play(RUN);
        }
    }
}
 
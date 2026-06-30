using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    //CLICK TO MOVE:
 private NavMeshAgent agent;
    private Animator anim;

    [Header("Movement")]
    public float walkSpeed = 3.5f;

    [Header("Input Setting")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float sampleDistance = 0.5f;

    public static event System.Action<Vector3> OnGroundTouch;
    private void Start()
    {
        //anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
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
                    agent.SetDestination(navHit.position);
                    OnGroundTouch?.Invoke(navHit.position);
                }
                
            }
            else
                Debug.Log("Clicked point is not on the NavMesh.");
        }
        ////Player Animation
        //float normalizedSpeed = Mathf.InverseLerp(0f, agent.speed, agent.velocity.magnitude);
        //anim.SetFloat("speed", normalizedSpeed);
    }


}
 
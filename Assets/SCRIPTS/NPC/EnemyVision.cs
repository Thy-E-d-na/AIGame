
using System.Collections;
using UnityEngine;
public class EnemyVision : MonoBehaviour
{
    [Range(1,15)]public float normalVision = 15f;   
    [Range(1,15)]public float normalFoV = 90f;
    [Range(1,5)]public float stealthVision = 5f;
    [Range(1,5)] public float stealthFoV = 70f;
    public GameObject visionCone;

    [Header("normal vision")]

    float visualRange; // vision range
    float viewAngle; // fov

    [Header("obstacles")]
    public Transform target; // player
    public LayerMask obstacleMask;

    public bool canSeeTarget;

    // delay time variable
    public float sensorTickRate = 0.5f;
    private void Start()
    {
        PlayerMovement player = FindFirstObjectByType<PlayerMovement>();
        player.OnStealthChanged += HandleStealthState;  
        StartCoroutine(FOVCoroutine());
    }
    void HandleStealthState(bool isStealth)
    {
        if(isStealth)
        {
            visualRange = stealthVision;
            viewAngle = stealthFoV;
           
        }
        else
        {
            visualRange = normalVision;
            viewAngle = normalFoV;

        }
    }
    void visionMesh()
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[3];
  

        vertices[0] = Vector3.zero;
        vertices[1] = DirFromAngle(-viewAngle / 2f, false) * visualRange; 
        vertices[2] = DirFromAngle(viewAngle / 2f, false) * visualRange;

        int[] triangles = new int[3] { 0, 1, 2 };

     
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();


        // Gan mesh vao visionCone
        MeshFilter mf = visionCone.GetComponent<MeshFilter>();
        MeshRenderer mr = visionCone.GetComponent<MeshRenderer>();

        mf.mesh = mesh;
        mr.enabled = true;

        // visionCone lm child de no xoay theo enemy
        visionCone.transform.localPosition = Vector3.zero;
        visionCone.transform.localRotation = Quaternion.identity;
    }



    IEnumerator FOVCoroutine()
    {
        var wait = new WaitForSeconds(sensorTickRate);
        while (true)
        {
            yield return wait;
            CheckFieldOfView();
        }
    }

    void CheckFieldOfView()
    {
        canSeeTarget = false;

        if (target == null) return;

      
        var distance = Vector3.Distance(transform.position, target.position);
        if (distance < visualRange)
        {
            // huong tu quai den player
            var direction = (target.position - transform.position).normalized;
            // kiem tra goc nhin
            var angle = Vector3.Angle(transform.forward, direction);
            if (angle < viewAngle / 2)
            {
                // kiem tra vat can
                if (!Physics.Raycast(transform.position,
                        direction, out RaycastHit hit, distance, obstacleMask))
                {
                    canSeeTarget = true;
                }
            }
        }

    }

    Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0,
            Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}


//private void OnDrawGizmos()
//{
//    Gizmos.color = Color.white;
//    Gizmos.DrawWireSphere(transform.position, viewRadius);

//    var viewAngleA = DirFromAngle(-viewAngle / 2f, false);
//    var viewAngleB = DirFromAngle(viewAngle / 2f, false);

//    // ve 2 tia tao thanh hinh non
//    Gizmos.DrawLine(transform.position, transform.position + viewAngleA * viewRadius);
//    Gizmos.DrawLine(transform.position, transform.position + viewAngleB * viewRadius);

//    // neu nhin thay player, ve 1 tia tu nhan vat den player
//    if (canSeeTarget)
//    {
//        Gizmos.color = Color.red;
//        Gizmos.DrawLine(transform.position, target.position);
//    }
//}

//// tinh huong vector tu goc
//Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
//{
//    if (!angleIsGlobal)
//    {
//        angleInDegrees += transform.eulerAngles.y;
//    }

//    return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0,
//        Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
//}
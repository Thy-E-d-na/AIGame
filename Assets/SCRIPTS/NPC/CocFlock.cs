using System.Collections.Generic;
using UnityEngine;

public class CocFlock : MonoBehaviour
{
    public float maxSpeed = 5f;
    public float maxForce = 5f;

    
    public float neighborRadius = 5f;

    public float separationRadius = 5f;

    [Range(1, 15)] public float separationWeight = 0.5f;

    [Range(1, 15)] public float alignmentWeight = 0.5f;

    [Range(1, 15)] public float cohesionWeight = 0.5f;

    [Range(1, 15)] public float seekWeight = 0.5f;


    public Transform target;
    public Transform player;
    public float fleeRadius = 3f;


    public LayerMask boidLayerMask;

    public Vector3 currentVelocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentVelocity = transform.forward;
        BehaviourTree npc = FindAnyObjectByType<BehaviourTree>();
       
    }

    void corpseEater()
    {

    }
    // Update is called once per frame
    void Update()
    {
        var flee = false;
        var find = Physics.OverlapSphere(transform.position, fleeRadius);
        foreach (var hit in find)
        {
            if (hit.CompareTag("Player"))
            {
                player = hit.transform;
                flee = true;
                break;
            }              
                
        }
        if(flee) Flee();
        else Flocking();


    }
    void Flocking()
    {
        var hitColliders = Physics.OverlapSphere(transform.position,
          neighborRadius, boidLayerMask);
        var neighbors = new List<Transform>();

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.transform != this.transform)
                neighbors.Add(hitCollider.transform);
        }

        var separationForce = CalculateSeparation(neighbors) * separationWeight;
        var alignmentForce = CalculateAlignment(neighbors) * alignmentWeight;
        var cohesionForce = CalculateCohesion(neighbors) * cohesionWeight;
        var seekForce = target != null ? Seek(target.position) * seekWeight : Vector3.zero;

        var totalForce = separationForce + alignmentForce + cohesionForce + seekForce;

        totalForce = Vector3.ClampMagnitude(totalForce, maxForce);


        currentVelocity += totalForce * Time.deltaTime;
        currentVelocity = Vector3.ClampMagnitude(currentVelocity, maxSpeed);

        transform.position += currentVelocity * Time.deltaTime;
        if (currentVelocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(currentVelocity);
        }
    }

    void Flee()
    {
        var velocity = (transform.position - player.position).normalized * maxSpeed;
        transform.position += (velocity - currentVelocity) * Time.deltaTime;
        
    }
    // luc keo ve muc tieu
    Vector3 Seek(Vector3 targetPosition)
    {
        var velocity = (targetPosition - transform.position).normalized * maxSpeed;
        
        return (velocity - currentVelocity);
    }

    Vector3 CalculateCohesion(List<Transform> neighbors)
    {
        if (neighbors.Count == 0) return Vector3.zero;
        var centerOfMass = Vector3.zero;
        foreach (var neighbor in neighbors)
        {
            centerOfMass += neighbor.position;
        }
        centerOfMass /= neighbors.Count;
        return Seek(centerOfMass);
    }

    Vector3 CalculateSeparation(List<Transform> neighbors)
    {
        var steering = Vector3.zero;
        var count = 0;
        foreach (var neighbor in neighbors)
        {
            var distance = Vector3.Distance(neighbor.position, transform.position);
            if (distance > 0 && distance < separationRadius)
            {
                var awayFromNeighbor = transform.position - neighbor.position;
                steering += awayFromNeighbor.normalized / distance;
                count++;
            }
        }

        if (count > 0)
        {
            steering /= count;
            steering = steering.normalized * maxSpeed;
            steering -= currentVelocity;
        }
        return steering;
    }

    // luc huong theo dam dong
    Vector3 CalculateAlignment(List<Transform> neighbors)
    {
        if (neighbors.Count == 0) return Vector3.zero;

        var averageForward = Vector3.zero;
        foreach (var neighbor in neighbors)
        {
            averageForward += neighbor.forward;
        }
        averageForward /= neighbors.Count;
        var velocity = averageForward.normalized * maxSpeed;
        return (velocity - currentVelocity);
    }


}

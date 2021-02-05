using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBoid : MonoBehaviour
{
    
    public Vector3 velocity;
    public float speed;
    public Vector3 acceleration;
    public Vector3 force;
    public float maxSpeed = 5;
    public float maxForce = 10;

    public float mass = 1;

    public Transform targetTransform;
    public Transform fleeTransform;
    
    public bool fleeEnabled = true;
    public Vector3 fleeTarget;
    
    public bool seekEnabled = true;
    public Vector3 seekTarget;

    public bool arriveEnabled = false;
    public Vector3 arriveTarget;
    public float slowingDistance = 10;

    public Path path;
    public float newTargetDistance = 0.5f;
    public int waypointTargetNum = 0;


    public void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + velocity);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + acceleration);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + force * 10);

        if (arriveEnabled)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(targetTransform.position, slowingDistance);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    
    public Vector3 Seek(Vector3 target)
    {
        Vector3 toTarget = target - transform.position;
        Vector3 desired = toTarget.normalized * maxSpeed;

        return (desired - velocity);
    } 

    public Vector3 Arrive(Vector3 target)
    {
        Vector3 toTarget = target - transform.position;
        float dist = toTarget.magnitude;
        float ramped = (dist / slowingDistance) * maxSpeed;
        float clamped = Mathf.Min(ramped, maxSpeed);
        Vector3 desired = (toTarget / dist) * clamped;

        return desired - velocity;
    }
    public Vector3 Flee(Vector3 target)
    {
        Vector3 toTarget = transform.position-target;
        float dist = toTarget.magnitude;
        float ramped = (10/dist) * maxSpeed;
        float clamped = Mathf.Min(ramped, maxSpeed);
        Vector3 desired = toTarget.normalized * clamped;

        return (desired - velocity)*2;
    } 

    public Vector3 CalculateForce()
    {
        Vector3 f = Vector3.zero;
        if (seekEnabled)
        {
            if (targetTransform != null)
            {
                seekTarget = targetTransform.position;
            }
            f += Seek(seekTarget);
        }

        if (arriveEnabled)
        {
            if (targetTransform != null)
            {
                arriveTarget = targetTransform.position;                
            }
            f += Arrive(arriveTarget);
        }
        
        
        if (fleeEnabled)
        {
            if (fleeTransform != null)
            {
                fleeTarget = fleeTransform.position;
                
            }
            if(Vector3.Distance(this.transform.position,fleeTarget)<10)
                f += Flee(fleeTarget);

        }

        return f;
    }

    public void FollowPath()
    {
        if (Vector3.Distance(this.transform.position, targetTransform.position) < newTargetDistance)
        {
            if (path.isLooped)
            {
                waypointTargetNum = (waypointTargetNum + 1) % path.waypoints.Count;
            }
            else
            {
                if (waypointTargetNum< path.waypoints.Count)
                {
                    waypointTargetNum += 1;
                }
            }
        }
        targetTransform.position = path.waypoints[waypointTargetNum];
    }

    // Update is called once per frame
    void Update()
    {
        force = CalculateForce();
        acceleration = force / mass;
        velocity = velocity + acceleration * Time.deltaTime;
        transform.position = transform.position + velocity * Time.deltaTime;
        speed = velocity.magnitude;
        if (speed > 0)
        {
            transform.forward = velocity;
        }
        FollowPath();
        
        
    }
}

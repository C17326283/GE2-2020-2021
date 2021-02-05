using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public List<Vector3> waypoints;
    public bool isLooped=true;
    
    // Start is called before the first frame update
    void Awake()
    {
        waypoints.Add(this.transform.position);
        foreach (Transform child in transform)
        {
            waypoints.Add(child.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrawGizmos()
    {
        for (int i = 0; i < waypoints.Count; i++)
        {
            Gizmos.color = Color.magenta;
            if(isLooped)
                Gizmos.DrawLine(waypoints[i], waypoints[(i+1)%waypoints.Count]);
            else if(i <waypoints.Count-1)
                Gizmos.DrawLine(waypoints[i], waypoints[i+1]);
        }
        
    }

}

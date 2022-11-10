using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// reference to code: https://www.youtube.com/watch?v=ly9mK0TGJJo
public class WaypointPath : MonoBehaviour
{
    public Transform GetWaypoint(int waypointIndex)
    {
        return transform.GetChild(waypointIndex);
    }

    public int GetNextWaypointIndex(int currentWaypointIndex)
    {
        int nextWaypointIndex = currentWaypointIndex + 1;
        if(nextWaypointIndex == transform.childCount)
        {
            nextWaypointIndex = 0;
        }
        return nextWaypointIndex;
    }
}

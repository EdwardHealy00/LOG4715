using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// reference to code: https://www.youtube.com/watch?v=ly9mK0TGJJo
public class PlatformController : MonoBehaviour
{
    [SerializeField] private WaypointPath waypointPath;
    [SerializeField] private float speed;

    private int targetWaypointIndex = 0;

    private Transform previousWaypoint;
    private Transform targetWaypoint;

    private float timeToWaypoint;
    private float elapsedTime;

    public bool leverPressed = false;

    private void Update()
    {
        if (!leverPressed) return;

        elapsedTime += Time.deltaTime;

        float perc = elapsedTime / timeToWaypoint;
        perc = Mathf.SmoothStep(0, 1, perc);

        transform.position = Vector3.Lerp(previousWaypoint.position, targetWaypoint.position, perc);
        transform.rotation = Quaternion.Lerp(previousWaypoint.rotation, targetWaypoint.rotation, perc);

        if (perc >= 1) TargetNextWaypoint();
    }

    public void TargetNextWaypoint()
    {
        previousWaypoint = waypointPath.GetWaypoint(targetWaypointIndex);
        targetWaypointIndex = waypointPath.GetNextWaypointIndex(targetWaypointIndex);
        targetWaypoint = waypointPath.GetWaypoint(targetWaypointIndex);

        elapsedTime = 0f;

        float distance = Vector3.Distance(previousWaypoint.position, targetWaypoint.position);
        timeToWaypoint = distance / speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(transform);
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }
}

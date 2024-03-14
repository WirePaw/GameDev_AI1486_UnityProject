using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using static UnityEditor.FilePathAttribute;

public class CanFollowPath : MonoBehaviour
{
    //attributes
    public float speed;
    public int currentWaypoint;
    public bool isMoving;
    public bool isWaiting;

    //references
    List<Transform> waypoints;
    List<Vector2> waypointPositions;
    List<CanCountdownTimer> waypointTimers;

    // make parent-object move towards waypoint
    public void MoveToWaypoint()
    {
        isMoving = true;
        StartCoroutine(Move(waypointPositions[currentWaypoint]));
    }
    public IEnumerator Move(Vector2 location)
    {

        float distance = ((Vector2)transform.parent.position - location).sqrMagnitude;
        while (distance > 0)
        {
            //print(Time.timeScale);
            transform.parent.position = Vector2.MoveTowards(transform.position, waypointPositions[currentWaypoint], speed * Time.deltaTime);
            distance = ((Vector2)transform.parent.position - location).sqrMagnitude;
            if (distance <= 0)
            {
                isMoving = false;
            }
            yield return new WaitUntil(() => Time.timeScale > 0);
        }
    }

    // wait for a specific amount of time (set in editor), and check if waiting is over
    public void Wait()
    {
        isMoving = false;
        isWaiting = true;

        waypointTimers[currentWaypoint].StartTimer();
    }
    public bool WaitingHasFinished()
    {
        return waypointTimers[currentWaypoint].HasFinished();
    }

    // setup and manage waypoints, used to form a path to walk
    private void SetupWaypoints()
    {
        waypoints = new List<Transform>();
        waypointPositions = new List<Vector2>();
        waypointTimers = new List<CanCountdownTimer>();

        foreach (Transform waypoint in transform)
        {
            waypoints.Add(waypoint);
            waypointPositions.Add(waypoint.position);
            waypointTimers.Add(waypoint.GetComponent<CanCountdownTimer>());
        }
        currentWaypoint = 0;
    }

    public Vector2 GetWaypointPosition()
    {
        return waypointPositions[currentWaypoint];
    }

    public void AdvanceToNextWaypoint()
    {
        currentWaypoint++;
        if (currentWaypoint > waypoints.Count-1)
        {
            currentWaypoint = 0;
        }
    }

    //-------------------------------------------------------------

    private void Awake()
    {
        SetupWaypoints();
    }
}
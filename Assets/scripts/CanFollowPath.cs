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

    //references
    List<Transform> waypoints;
    List<Vector2> waypointPositions;
    List<CanCountdownTimer> waypointTimers;

    //methods (actions?)

    public void MoveToWaypoint()
    {
        isMoving = true;
        StartCoroutine(Move(waypointPositions[currentWaypoint]));
    }

    public void Wait()
    {
        isMoving = false;
        waypointTimers[currentWaypoint].StartTimer();
    }

    public bool WaitingHasFinished()
    {
        return waypointTimers[currentWaypoint].HasFinished();
    }

    private void SetupWaypoints()
    {
        foreach (Transform waypoint in base.transform)
        {
            waypoints.Add(waypoint);
            waypointPositions.Add(waypoint.position);
            waypointTimers.Add(waypoint.GetComponent<CanCountdownTimer>());
        }
        currentWaypoint = 0;
    }

    public Vector2 GetNextPosition()
    {
        return waypointPositions[currentWaypoint];
    }

    public IEnumerator Move(Vector2 location)
    {
        float distance = ((Vector2)transform.position - location).sqrMagnitude;
        while (distance > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, waypointPositions[currentWaypoint], speed * Time.fixedDeltaTime);
            distance = ((Vector2)transform.position - location).sqrMagnitude;

            if(distance <= 0)
            {
                isMoving = false;
                currentWaypoint++;
                if (currentWaypoint >= waypoints.Count)
                {
                    currentWaypoint = 0;
                }
            }
            yield return null;
        }
    }

    //-------------------------------------------------------------

    private void Awake()
    {
        SetupWaypoints();
    }
}




/*

    public String state;
    public int nextPointIndex;
    public float speed;
    private _AnimationManager sprite;
    private CanSeePlayer sightcone;
    private CanCountdownTimer timer;
    public List<Transform> waypoints;
    public List<Vector3> waypointPositions; // <- quick solution to moving waypoints issue
    public Vector3 direction;
    private Vector3 oldPosition;

    private void Awake()
    {
        sprite = GetComponentInChildren<_AnimationManager>();
        sightcone = GetComponentInChildren<CanSeePlayer>();
        state = "standing";
        nextPointIndex = 0;
        waypoints = new List<Transform>();
        waypointPositions = new List<Vector3>();

    }
    private void Start()
    {
        // add any waypoints, stored as children under the path.gameObject

        foreach (Transform waypoint in transform.Find("path").transform)
        {
            waypoints.Add(waypoint);
            waypointPositions.Add(waypoint.position);
        }
        if (waypoints.Count > 0)
        {
            timer = waypoints[nextPointIndex].GetComponent<CanCountdownTimer>();
        }
    }

    void FixedUpdate()
    {
        if(waypoints.Count > 1)
        {
            if (!(transform.position == waypointPositions[nextPointIndex])) //TODO use Mathf.Approximately() to proper compare floats
            {
                state = "walking";
                oldPosition = transform.position;
                transform.position = Vector2.MoveTowards(transform.position, waypointPositions[nextPointIndex], speed * Time.fixedDeltaTime);
                direction = (transform.position - oldPosition).normalized;
                sightcone.setDirection(direction);
            }
            else
            {
                state = "waiting";
                timer.startTimer();

                // TODO redo timer for better readability
                if (!timer.hasStarted() && timer.hasEnded())
                {
                    timer.resetTimer();
                    nextPointIndex = (nextPointIndex + 1) % waypoints.Count;
                    timer = waypoints[nextPointIndex].GetComponent<CanCountdownTimer>();
                }
            }

            if (sprite != null)
            {
                sprite.setState(direction);
            }
        }
    }
}
*/
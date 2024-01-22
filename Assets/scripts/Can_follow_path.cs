using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Can_follow_path : MonoBehaviour
{
    public String state;
    public int nextPointIndex;
    public float speed;
    private AnimationManager sprite;
    private Can_see_player sightcone;
    private Can_count_down_timer timer;
    public List<Transform> waypoints;
    public List<Vector3> waypointPositions; // <- quick solution to moving waypoints issue
    public Vector3 direction;
    private Vector3 oldPosition;

    private void Start()
    {
        sprite = GetComponentInChildren<AnimationManager>();
        sightcone = GetComponentInChildren<Can_see_player>();
        state = "standing";
        speed = 1f;
        nextPointIndex = 0;
        waypoints = new List<Transform>();
        waypointPositions = new List<Vector3>();

        // add any waypoints, stored as children under the path.gameObject

        foreach (Transform waypoint in transform.Find("path").transform)
        {
            waypoints.Add(waypoint);
            waypointPositions.Add(waypoint.position);
        }
        if(waypoints.Count > 0)
        {
            timer = waypoints[nextPointIndex].GetComponent<Can_count_down_timer>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(waypoints.Count > 1)
        {
            if (!transform.position.Equals(waypointPositions[nextPointIndex])) // move towards next waypoint and adjust direction
            {
                state = "walking";
                oldPosition = transform.position;
                //TODO print("old: "+ oldPosition +" new: "+ )
                transform.position = Vector2.MoveTowards(transform.position, waypointPositions[nextPointIndex], speed * Time.fixedDeltaTime);
                direction = (transform.position - oldPosition).normalized;
                sightcone.setDirection(direction);
            }
            else
            {
                state = "waiting";
                timer.startTimer();

                if (!timer.hasStarted() && timer.hasEnded())
                {
                    timer.resetTimer();
                    nextPointIndex = (nextPointIndex + 1) % waypoints.Count;
                    timer = waypoints[nextPointIndex].GetComponent<Can_count_down_timer>();
                }
            }

            if (sprite != null)
            {
                sprite.setState(direction);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steerings;
using FSM;
using Pathfinding;

[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(PathFollowing))]
public class FMS_PathExecution : FiniteStateMachine
{
    public enum PathExecutionStates { INITIAL, GENERATING, FOLLOWING, TERMINATED }
    private PathExecutionStates currentState;

    private Seeker seeker;
    private PathFollowing pathFollowing;

    public GameObject Target;
    public float CloseEnoughRadius;
    public Path CurrentPath;
    public bool Terminated;
    void Start()
    {
        seeker = GetComponent<Seeker>();
        pathFollowing = GetComponent<PathFollowing>();

        pathFollowing.enabled = false;

        currentState = PathExecutionStates.INITIAL;
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void ReEnter()
    {
        base.ReEnter();
        Terminated = false;
    }
    void Update()
    {
        switch (currentState)
        {
            case (PathExecutionStates.INITIAL):
                ChangeState(PathExecutionStates.GENERATING);
                break;
            case (PathExecutionStates.GENERATING):
                ChangeState(PathExecutionStates.FOLLOWING);
                break;
            case (PathExecutionStates.FOLLOWING):
                if (SensingUtils.DistanceToTarget(gameObject, Target) <= CloseEnoughRadius)
                {
                    ChangeState(PathExecutionStates.TERMINATED);
                }
                break;
            case (PathExecutionStates.TERMINATED):
                break;
        }
    }
    private void ChangeState(PathExecutionStates newState)
    {
        switch (currentState)
        {
            case (PathExecutionStates.GENERATING):
                break;
            case (PathExecutionStates.FOLLOWING):
                pathFollowing.enabled = false;
                break;
            case (PathExecutionStates.TERMINATED):
                break;
        }
        switch (newState)
        {
            case (PathExecutionStates.GENERATING):
                seeker.StartPath(transform.position, Target.transform.position, OnPathComplete);
                break;
            case (PathExecutionStates.FOLLOWING):
                pathFollowing.enabled = true;
                break;
            case (PathExecutionStates.TERMINATED):
                Terminated = true;
                break;
        }
        currentState = newState;
    }
    public void OnPathComplete(Path p)
    {
        CurrentPath = p;

        pathFollowing.path = CurrentPath;
        pathFollowing.currentWaypointIndex = 0;
    }
}

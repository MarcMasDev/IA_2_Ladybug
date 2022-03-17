using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using Pathfinding;

[RequireComponent(typeof(Ant_BLACKBOARD))]
public class FMS_Ant : FiniteStateMachine
{
    public enum AntStates { INITIAL, DELIVER, EXIT}
    public AntStates currentState;

    private FMS_PathExecution fMS_PathExecution;
    private GameObject target;
    public GameObject Load;
    private Ant_BLACKBOARD blackBoard;
    // Start is called before the first frame update
    void Start()
    {
        blackBoard = GetComponent<Ant_BLACKBOARD>();
        fMS_PathExecution = GetComponent<FMS_PathExecution>();
        currentState = AntStates.INITIAL;

        fMS_PathExecution.Exit();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case AntStates.INITIAL:
                    ChangeState(AntStates.DELIVER);
                break;
            case AntStates.DELIVER:
                if (fMS_PathExecution.Terminated)
                {
                    GraphNode node = AstarPath.active.GetNearest(Load.transform.position,
                        NNConstraint.Default).node;
                    Load.transform.position = (Vector3)node.position;
                    Load.transform.parent = null;
                    if (Load.tag == blackBoard.seedOnAnt)
                    {
                        Load.tag = blackBoard.seed;
                    }
                    else if (Load.tag == blackBoard.eggOnAnt)
                    {
                        Load.tag = blackBoard.egg;
                    }
                    ChangeState(AntStates.EXIT);
                }
                break;
            case AntStates.EXIT:
                if (fMS_PathExecution.Terminated)
                {
                    Destroy(gameObject);
                }
                break;
        }
    }
    private void ChangeState(AntStates newState)
    {
        switch (currentState)
        {
            case AntStates.DELIVER:
                fMS_PathExecution.Exit();
                break;
            case AntStates.EXIT:
                break;
        }

        switch (newState)
        {
            case AntStates.DELIVER:
                target = SensingUtils.FindRandomInstanceWithinRadius(gameObject, "NODE", 100);
                fMS_PathExecution.ReEnter();
                fMS_PathExecution.Target = target;
                break;
            case AntStates.EXIT:
                target = blackBoard.exitPoints[Random.Range(0, blackBoard.exitPoints.Length - 1)];
                fMS_PathExecution.ReEnter();
                fMS_PathExecution.Target = target;
                break;
        }
        currentState = newState;
    }
}

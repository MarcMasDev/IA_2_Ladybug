using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;

public class FMS_Ant : FiniteStateMachine
{
    public enum AntStates { INITIAL, DELIVER, EXIT}
    public AntStates currentState;

    private FMS_PathExecution fMS_PathExecution;
    private GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        fMS_PathExecution.GetComponent<FMS_PathExecution>();
        currentState = AntStates.INITIAL;
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
                ChangeState(AntStates.EXIT);
                break;
            case AntStates.EXIT:
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
                fMS_PathExecution.Exit();
                Destroy(gameObject);
                break;
        }

        switch (newState)
        {
            case AntStates.DELIVER:
                fMS_PathExecution.ReEnter();
                fMS_PathExecution.Target = target;
                break;
            case AntStates.EXIT:
                fMS_PathExecution.ReEnter();
                fMS_PathExecution.Target = target;
                break;
        }
        currentState = newState;
    }
}

using FSM;
using Steerings;
using UnityEngine;

public class FMS_LadyBug : FiniteStateMachine
{
    public enum LadyStates { INITIAL, WANDERING, REACHING_EGG, REACHING_SEED, TRANSPORTING }
    public LadyStates currState;

    private WanderAround wanderAround;
    private Arrive arrive;
    private Lady_BLACKBOARD blackBoard;

    //private float elapsedTime;

    private GameObject target;
    private GameObject place;

    private void Start()
    {
        blackBoard = GetComponent<Lady_BLACKBOARD>();
        wanderAround = GetComponent<WanderAround>();
        arrive = GetComponent<Arrive>();

        wanderAround.enabled = false;
        arrive.enabled = false;
    }
    public override void Exit()
    {
        wanderAround.enabled = false;
        arrive.enabled = false;

        target.transform.parent = null;

        this.enabled = false;
    }
    private void Update()
    {
        switch (currState)
        {
            case LadyStates.INITIAL:
                ChangeState(LadyStates.WANDERING);
                break;
            case LadyStates.WANDERING:

                //looking for an egg and seed
                GameObject egg = SensingUtils.FindInstanceWithinRadius(gameObject, blackBoard.eggTag, blackBoard.eggMinDistance);
                GameObject seed = SensingUtils.FindInstanceWithinRadius(gameObject, blackBoard.seedTag, blackBoard.seedMinDistance);

                if (egg == null) //&& seed == null) //No hay huevo ni semilla cerca. Vamos a ver primero si hay un huevo a 180u
                {
                    egg = SensingUtils.FindRandomInstanceWithinRadius(gameObject, blackBoard.eggTag, blackBoard.eggMaxDistance);

                    if (egg != null)
                    {
                        target = egg;
                        ChangeState(LadyStates.REACHING_EGG);
                    }

                    if (seed == null && egg == null)//Lo mismo que le primer If pero damos prioridad a la semilla aquí.
                    {
                        seed = SensingUtils.FindRandomInstanceWithinRadius(gameObject, blackBoard.seedTag, blackBoard.seedMaxDistance);
                        if (seed != null)
                        {
                            target = seed;
                            ChangeState(LadyStates.REACHING_SEED);
                        }
                    }
                    else if (seed != null && egg == null) //Se detecta semilla en 80u y no hay huevo.
                    {
                        target = seed;
                        ChangeState(LadyStates.REACHING_SEED);
                    }
                }
                else if (egg != null) //En el caso de que sí exista un huevo a 50unidades.
                {
                    target = egg;
                    egg = null;
                    ChangeState(LadyStates.REACHING_EGG);
                }
                

                break;
            case LadyStates.REACHING_EGG:

                GameObject nearestEgg = SensingUtils.FindInstanceWithinRadius(gameObject, blackBoard.eggTag, blackBoard.eggMinDistance);
                float currTargetDistance = SensingUtils.DistanceToTarget(gameObject, target);
                if (nearestEgg != null && nearestEgg != target)
                {
                    float possibleTargetDistance = SensingUtils.DistanceToTarget(gameObject, nearestEgg);
                    if (possibleTargetDistance < currTargetDistance)
                    {
                        target = nearestEgg;
                        arrive.target = target;
                    }
                }

                if (currTargetDistance <= blackBoard.transportingDistance)
                {
                    place = blackBoard.hatchingChamber;
                    ChangeState(LadyStates.TRANSPORTING);
                }
                else if (currTargetDistance > blackBoard.eggMaxDistance || target.tag != "EGG")
                    ChangeState(LadyStates.WANDERING);

                break;
            case LadyStates.REACHING_SEED:

                if (IsAnEggNear())
                    ChangeState(LadyStates.REACHING_EGG);

                if (SensingUtils.DistanceToTarget(gameObject, target) <= blackBoard.transportingDistance)
                {
                    place = blackBoard.storeChamber;
                    ChangeState(LadyStates.TRANSPORTING);
                }
                else if (SensingUtils.DistanceToTarget(gameObject, target) > blackBoard.seedMaxDistance || target.tag != "SEED")
                    ChangeState(LadyStates.WANDERING);

                break;
            case LadyStates.TRANSPORTING:
               
                if (target.CompareTag(blackBoard.seedTransportedTag) && IsAnEggNear())
                    ChangeState(LadyStates.REACHING_EGG);
                else if (SensingUtils.DistanceToTarget(gameObject, place) < blackBoard.transportingDistance)
                    ChangeState(LadyStates.WANDERING);

                break;
            default:
                break;
        }
    }

    private void ChangeState(LadyStates newState)
    {
        switch (currState)
        {
            case LadyStates.INITIAL:
                break;
            case LadyStates.WANDERING:
                wanderAround.enabled = false;
                break;
            case LadyStates.REACHING_EGG:
                arrive.enabled = false;
                break;
            case LadyStates.REACHING_SEED:
                arrive.enabled = false;
                break;
            case LadyStates.TRANSPORTING:
                place = null;
                arrive.enabled = false;
                target.transform.parent = null;
                arrive.target = null;
                break;

            default:
                break;
        }

        switch (newState)
        {
            case LadyStates.INITIAL:
                break;
            case LadyStates.WANDERING:
                wanderAround.attractor = blackBoard.attractor;
                wanderAround.enabled = true;
                break;
            case LadyStates.REACHING_EGG:
                arrive.enabled = true;
                arrive.target = target;

                break;
            case LadyStates.REACHING_SEED:
                arrive.enabled = true;
                arrive.target = target;
                break;
            case LadyStates.TRANSPORTING:
                arrive.target = place;
                if (target.CompareTag(blackBoard.seedTag))
                    target.tag = blackBoard.seedTransportedTag;
                else
                    target.tag = "Untagged";

                target.transform.parent = gameObject.transform;
                break;

                
            default:
                break;
        }
        currState = newState;
    }

    private bool IsAnEggNear()
    {
        GameObject nearEgg = SensingUtils.FindInstanceWithinRadius(gameObject, blackBoard.eggTag, blackBoard.eggMinDistance / 2);
        
        if (nearEgg != null)
        {
            target.transform.parent = null;
            target.tag = blackBoard.seedTag;
            target = nearEgg;
            return true;
        }
        return false;
    }
}


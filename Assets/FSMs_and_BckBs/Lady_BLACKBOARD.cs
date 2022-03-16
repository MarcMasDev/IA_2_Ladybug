using UnityEngine;

public class Lady_BLACKBOARD : MonoBehaviour
{
    [Header("LadyBug")]
    public GameObject hatchingChamber, storeChamber;
    [Space(5)]
    public float IntervalBetweenIncrements;
    public float MinimumSeekWeight;
    public float SeekWeightDecrease;
    [Space(5)]
    public float seedMinDistance = 80;
    public float seedMaxDistance = 125;
    public float eggMinDistance=50;
    public float eggMaxDistance=180;
    public float transportingDistance=5;

    [Space(5)]
    [Header("TAGS")]
    public string eggTag;
    public string seedTag;
    public string seedTransportedTag;
    public string nestChamber;
    public string seedChamber;
    [Tooltip("where ants can moves and throws the seed")]
    public string PathNode;
  

    //[Space(20)]
    
}

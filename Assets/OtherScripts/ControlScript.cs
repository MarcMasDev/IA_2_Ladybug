using Pathfinding;
using UnityEngine;

public class ControlScript : MonoBehaviour
{
    private Camera cam;
    private GameObject eggPrefab;
    private GameObject seedPrefab;
    

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        eggPrefab = Resources.Load<GameObject>("EGG");
        seedPrefab = Resources.Load<GameObject>("SEED");
      

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0) && Input.GetKey("s"))
        {
            var position = cam.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0;

            GraphNode node = AstarPath.active.GetNearest(position, NNConstraint.Default).node;
            position = (Vector3)node.position;

            GameObject seed = GameObject.Instantiate(seedPrefab);
            seed.transform.position = position;
            // seed.transform.Rotate(0, 0, Random.value * 360);
            seed.tag = "SEED";
        }
        

 
        if (Input.GetMouseButtonDown(0) && Input.GetKey("e"))
        {
            var position = cam.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0;

            GraphNode node = AstarPath.active.GetNearest(position, NNConstraint.Default).node;
            position = (Vector3)node.position;

            GameObject egg = GameObject.Instantiate(eggPrefab);
            egg.transform.position = position;
            egg.tag = "EGG";
        }

        

    }
}

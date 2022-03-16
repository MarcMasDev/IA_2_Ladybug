using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntSpawner : MonoBehaviour 
{
    public GameObject eggAnt;
    public GameObject seedAnt;
    public float eggProb;
    public float minTime;
    public float maxTime;
    private float time;
    private float timer;
    private void Start()
    {
        time = Random.Range(minTime, maxTime);
    }
    private void Update()
    {
        if (timer >= time)
        {
            if (Random.value <= eggProb)
            {
                Instantiate(eggAnt, transform.position, Quaternion.identity).SetActive(true);
            }
            else
            {
                Instantiate(seedAnt, transform.position, Quaternion.identity).SetActive(true);
            }
            time = Random.Range(minTime, maxTime);
            timer = 0;
        }
        timer += Time.deltaTime;
    }
}

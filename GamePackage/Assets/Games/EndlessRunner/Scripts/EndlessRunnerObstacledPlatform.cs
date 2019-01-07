using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessRunnerObstacledPlatform : EndlessRunnerPlatform
{
    public GameObject[] Obstacles;
    public List<GameObject> Positions;

    protected override void Start()
    {
        base.Start();
        foreach (GameObject obstacle in Obstacles)
        {
            int index = UnityEngine.Random.Range(0, Positions.Count);
            obstacle.transform.position = Positions[index].transform.position;
            Positions.RemoveAt(index);
            obstacle.SetActive(true);
        }
    }
}

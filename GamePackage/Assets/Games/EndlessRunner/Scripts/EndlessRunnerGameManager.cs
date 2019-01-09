using System.Collections;
using System.Collections.Generic;
using NTBUtils;
using UnityEngine;

public class EndlessRunnerGameManager : SingletonBehavior<EndlessRunnerGameManager>
{
    public void StartGame()
    {
        EndlessRunnerPlayer.Instance.enabled = true;
        EndlessRunnerStoryteller.Instance.enabled = true;
        EndlessRunnerStoryteller.Instance.PlayGame();
    }

    public void EndGame()
    {
        EndlessRunnerPlayer.Instance.enabled = false;
        EndlessRunnerStoryteller.Instance.enabled = false;
    }
}

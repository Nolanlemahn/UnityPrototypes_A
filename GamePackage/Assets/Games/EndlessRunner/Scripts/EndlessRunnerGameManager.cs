using System.Collections;
using System.Collections.Generic;
using NTBUtils;
using UnityEngine;
using UnityEngine.UI;

public class EndlessRunnerGameManager : SingletonBehavior<EndlessRunnerGameManager>
{
    public Text Score;
    public Text Speed;
    public Button PlayButton;
    public Button ResetButton;

    void Update()
    {
        this.Score.text = EndlessRunnerPlayer.Instance.DistanceTravelled.ToString();
        this.Speed.text = EndlessRunnerPlayer.Instance.RB.velocity.magnitude.ToString();
    }

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
        this.ResetButton.gameObject.SetActive(true);
    }

    public void ResetGame()
    {
        EndlessRunnerPlayer.Instance.Restart();
        EndlessRunnerStoryteller.Instance.Restart();
        this.PlayButton.gameObject.SetActive(true);
    }
}

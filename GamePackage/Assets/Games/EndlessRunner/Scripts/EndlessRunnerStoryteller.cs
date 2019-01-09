using System.Collections;
using System.Collections.Generic;
using NTBUtils;
using UnityEngine;

[System.Serializable]
public struct PlatformDistribution
{
    public float Weight;
    public EndlessRunnerPlatform PlatformPrefab;
}

public class EndlessRunnerStoryteller : SingletonBehavior<EndlessRunnerStoryteller>
{
    [Header("Runtime Parameters")]
    [Tooltip("Cheat on first platform")] public float CurrentPlatformWidth;
    public float NormalTimer;
    public float DistanceAhead;
    public List<PlatformDistribution> Platforms;
    /*
    [CurveAttribute(-5f, 5f, -5f, 5f,
        0f, 1f, 0f, 1f)]
    public AnimationCurve AC;
    */
    private float _totalWeight = 0f;
    private IEnumerator _currentGameplayCoroutine = null;

    public void PlayGame(List<PlatformDistribution> platforms = null)
    {
        if (this._currentGameplayCoroutine != null)
        {
            Debug.LogWarning("Game is already playing.");
            return;
        }
        if (platforms != null) this.Platforms = platforms;
        Platforms.ForEach(plat => _totalWeight += plat.Weight);
        this._currentGameplayCoroutine = this.spawnPlatformCoroutine();
        this.StartCoroutine(this._currentGameplayCoroutine);
    }

    private void OnDisable()
    {
        this._totalWeight = 0f;
        this.StopCoroutine(this._currentGameplayCoroutine);
    }

    private IEnumerator spawnPlatformCoroutine()
    {
        while (true)
        {
            EndlessRunnerPlatform selectedPlatform = this.randomPlatform();

            yield return new WaitForSeconds(this.NormalTimer +
                this.CurrentPlatformWidth / EndlessRunnerPlayer.Instance.ForwardSpeed);

            this.CurrentPlatformWidth = selectedPlatform.RealWidth;

            this.spawnPlatform(selectedPlatform);
        }
    }

    private EndlessRunnerPlatform randomPlatform()
    {
        float roll = Random.Range(0f, _totalWeight);
        float summedWeight = 0f;
        foreach (PlatformDistribution platform in Platforms)
        {
            float newSummedWeight = summedWeight + platform.Weight;
            if (summedWeight <= roll && roll <= newSummedWeight)
                return platform.PlatformPrefab;
            summedWeight = newSummedWeight;
        }
        return null;
    }

    private void spawnPlatform(EndlessRunnerPlatform platform)
    {
        Vector3 pos = EndlessRunnerPlayer.Instance.transform.position;
        pos.y = 0;
        pos.z += this.DistanceAhead;

        Instantiate(platform.gameObject, pos, Quaternion.identity);
    }
}

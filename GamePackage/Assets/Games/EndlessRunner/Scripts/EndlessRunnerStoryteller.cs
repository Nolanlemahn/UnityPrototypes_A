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
    private IEnumerator _storyCoroutine = null;

    private void OnEnable()
    {
        this._storyCoroutine = this.spawnPlatformCoroutine();
        this.StartCoroutine(this._storyCoroutine);
        Platforms.ForEach(plat => _totalWeight += plat.Weight);
    }

    private void OnDisable()
    {
        this.StopCoroutine(this._storyCoroutine);
        _totalWeight = 0;
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

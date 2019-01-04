using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlatformDistribution
{
    public float Weight;
    public EndlessRunnerPlatform PlatformPrefab;
}

public class EndlessRunnerStoryteller : MonoBehaviour
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

    void Start()
    {
        this.StartCoroutine(this.spawnPlatformCoroutine());
    }

    private IEnumerator spawnPlatformCoroutine()
    {
        while (true)
        {
            EndlessRunnerPlatform selectedPlatform = this.Platforms[0].PlatformPrefab;

            yield return new WaitForSeconds(this.NormalTimer +
                this.CurrentPlatformWidth / EndlessRunnerPlayer.Instance.Speed);

            this.CurrentPlatformWidth = selectedPlatform.RealWidth;

            this.spawnPlatform(selectedPlatform);
        }
    }

    private void spawnPlatform(EndlessRunnerPlatform platform)
    {
        Vector3 pos = EndlessRunnerPlayer.Instance.transform.position;
        pos.y = 0;
        pos.z += this.DistanceAhead;

        Instantiate(platform.gameObject, pos, Quaternion.identity);
    }
}

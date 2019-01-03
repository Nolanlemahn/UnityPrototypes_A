using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EndlessRunnerPlatform : MonoBehaviour
{
    public float RealWidth = 1f;
    public float OutOfBoundsLifetime = 5f;
    public Collider Collider;

    private IEnumerator _deathCoroutine = null;
	void Start ()
	{
	    if (!this.Collider) this.Collider = this.GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    Plane[] planes =
	        GeometryUtility.CalculateFrustumPlanes(EndlessRunnerPlayer.Instance.Camera);
	    if (!GeometryUtility.TestPlanesAABB(planes, this.Collider.bounds))
	    {
            //invisible
	        if (this._deathCoroutine == null)
	        {
	            this._deathCoroutine = this.beginDeathCountdown();
	            this.StartCoroutine(this._deathCoroutine);
	        }
	    }
	    else
	    {
            //visible
            this.StopAllCoroutines();
	        this._deathCoroutine = null;
	    }
    }

    private IEnumerator beginDeathCountdown()
    {
        yield return new WaitForSeconds(this.OutOfBoundsLifetime);
        Destroy(this.gameObject);
    }
}

using NTBUtils;
using UnityEngine;

public class EndlessRunnerPlayer : SingletonBehavior<EndlessRunnerPlayer>
{
    public Rigidbody RB;
    public Camera Camera;
    [Header("Body Parts")] public GameObject LFoot;
    public GameObject RFoot;

    private EndlessRunnerPlayerFeet _lFootScript;
    private EndlessRunnerPlayerFeet _rFootScript;

    private void Start()
    {
        this.RB.velocity = new Vector3(5, 0, 0);

        this._lFootScript = this.LFoot.AddComponent<EndlessRunnerPlayerFeet>();
        this._rFootScript = this.RFoot.AddComponent<EndlessRunnerPlayerFeet>();
    }

    private void Update()
    {
        SDbgPanel.Log("Player Velocity", this.RB.velocity);

        Vector3 vel = this.RB.velocity;

        if (this._lFootScript.CheckGrounded() || this._rFootScript.CheckGrounded())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                vel.y = 7f;
                this.RB.velocity = vel;
            }
        }
    }

    public class EndlessRunnerPlayerFeet : MonoBehaviour
    {
        public float Distance = 0.005f;

        public bool CheckGrounded()
        {
            RaycastHit hit;
            return Physics.Raycast(this.transform.position, Vector3.down, out hit, this.Distance);
        }
    }
}
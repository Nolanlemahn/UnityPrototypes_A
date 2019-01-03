using NTBUtils;
using UnityEngine;
using UnityEngine.Experimental.Input;

public class EndlessRunnerPlayer : SingletonBehavior<EndlessRunnerPlayer>
{
    public Rigidbody RB;
    public Camera Camera;
    public InputAction JumpAction;
    [Header("Body Parts")]
    public GameObject LFoot;
    public GameObject RFoot;

    private EndlessRunnerPlayerFeet _lFootScript;
    private EndlessRunnerPlayerFeet _rFootScript;

    protected override void Awake()
    {
        base.Awake();
        this.RB.velocity = new Vector3(5, 0, 0);

        this._lFootScript = this.LFoot.AddComponent<EndlessRunnerPlayerFeet>();
        this._rFootScript = this.RFoot.AddComponent<EndlessRunnerPlayerFeet>();

        this.JumpAction.performed += _ => this.tryJump();
    }

    private void OnEnable()
    {
        this.JumpAction.Enable();
    }

    private void OnDisable()
    {
        this.JumpAction.Enable();
    }

    private void tryJump()
    {
        if (this._lFootScript.CheckGrounded() || this._rFootScript.CheckGrounded())
        {
            Vector3 vel = this.RB.velocity;
            vel.y = 7f;
            this.RB.velocity = vel;
        }
    }

    private void Update()
    {
        SDbgPanel.Log("Player Velocity", this.RB.velocity);
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
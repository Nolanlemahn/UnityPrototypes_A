using NTBUtils;
using UnityEngine;
using UnityEngine.Experimental.Input;

// Demonstrates nested MonoBehavior, new InputSystem

public class EndlessRunnerPlayer : SingletonBehavior<EndlessRunnerPlayer>
{
    #region Properties

    [Header("Gameplay")]
    public float Speed = 5f;
    [Header("Components/Children/Etc")]
    public Rigidbody RB;
    public Camera Camera;
    public InputActionAsset MovementAsset;
    [Header("Body Parts")]
    public GameObject LFoot;
    public GameObject RFoot;
    [Header("Runtime variables")]
    public float DistanceTravelled = 0f;

    private EndlessRunnerPlayerFeet _lFootScript;
    private EndlessRunnerPlayerFeet _rFootScript;
    private Vector3 _initialPosition;

    private InputAction _jumpAction;
    private InputAction _moveAction;
    private float _movement = 0f;
    #endregion

    #region Lifecycle
    protected override void Awake()
    {
        base.Awake();
        this.RB.velocity = new Vector3(0, 0, this.Speed);

        this._lFootScript = this.LFoot.AddComponent<EndlessRunnerPlayerFeet>();
        this._rFootScript = this.RFoot.AddComponent<EndlessRunnerPlayerFeet>();
        this._initialPosition = this.transform.position;

        this._jumpAction = this.MovementAsset.actionMaps[0].actions[0];
        this._jumpAction.performed += _ => this.tryJump();
        this._moveAction = this.MovementAsset.actionMaps[0].actions[1];
        this._moveAction.performed += this.tryMove;
    }

    private void OnEnable()
    {
        this._jumpAction.Enable();
        this._moveAction.Enable();
    }

    private void OnDisable()
    {
        this._jumpAction.Disable();
        this._moveAction.Disable();
    }

    private void Update()
    {
        Vector3 pos = this.transform.position;
        pos.x += this._movement * Time.deltaTime;
        this.transform.position = pos;

        this.DistanceTravelled = this.transform.position.z - this._initialPosition.z;

        SDbgPanel.Log("Player Velocity", this.RB.velocity);
        SDbgPanel.Log("Player Distance", this.DistanceTravelled);
    }
    #endregion

    #region Helpers
    private void tryMove(InputAction.CallbackContext input)
    {
        this._movement = input.ReadValue<float>();
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

    public class EndlessRunnerPlayerFeet : MonoBehaviour
    {
        public float Distance = 0.005f;

        public bool CheckGrounded()
        {
            RaycastHit hit;
            return Physics.Raycast(this.transform.position, Vector3.down, out hit, this.Distance);
        }
    }
    #endregion
}
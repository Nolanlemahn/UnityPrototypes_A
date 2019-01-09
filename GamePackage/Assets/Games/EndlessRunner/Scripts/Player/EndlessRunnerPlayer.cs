using System.Collections.Generic;
using NTBUtils;
using UnityEngine;
using UnityEngine.Experimental.Input;
#if UNITY_EDITOR
using UnityEditor;
#endif

// Demonstrates nested MonoBehavior, new InputSystem

public class EndlessRunnerPlayer : SingletonBehavior<EndlessRunnerPlayer>
{
    #region Properties

    [Header("Gameplay")]
    public float ForwardSpeed = 5f;
    public float SidewaysSpeed = 1f;
    public float Y_KillLine = -0.5f;
    public float KillVelocity = 0.05f;
    [Header("Components/Children/Etc")]
    public Rigidbody RB;
    public Camera Camera;
    public InputActionAsset MovementAsset;
    [Header("Body Parts")]
    public GameObject FrontLFoot;
    public GameObject FrontRFoot;
    public GameObject BackLFoot;
    public GameObject BackRFoot;
    [Header("Runtime variables")]
    public float DistanceTravelled = 0f;

    private List<EndlessRunnerPlayerFeet> _feetScripts;
    private Vector3 _initialPosition;

    private InputAction _jumpAction;
    private InputAction _moveAction;
    private float _movement = 0f;
    #endregion

    #region Lifecycle
    protected override void Awake()
    {
        base.Awake();

        this._feetScripts = new List<EndlessRunnerPlayerFeet>
        {
            this.FrontLFoot.AddComponent<EndlessRunnerPlayerFeet>(),
            this.FrontRFoot.AddComponent<EndlessRunnerPlayerFeet>(),
            this.BackLFoot.AddComponent<EndlessRunnerPlayerFeet>(),
            this.BackRFoot.AddComponent<EndlessRunnerPlayerFeet>()
        };
        this._initialPosition = this.transform.position;

        this._jumpAction = this.MovementAsset.actionMaps[0].actions[0];
        this._jumpAction.performed += _ => this.tryJump();
        this._moveAction = this.MovementAsset.actionMaps[0].actions[1];
        this._moveAction.performed += this.tryMove;
    }

    private void OnEnable()
    {
        this.RB.useGravity = true;
        this.RB.velocity = new Vector3(0, 0, this.ForwardSpeed);
        this._jumpAction.Enable();
        this._moveAction.Enable();
    }

    private void OnDisable()
    {
        this.RB.useGravity = false;
        this.RB.velocity = Vector3.zero;
        this._jumpAction.Disable();
        this._moveAction.Disable();
    }

    private void Update()
    {
        // data polling
        Vector3 pos = this.transform.position;
        SDbgPanel.Log("Player Velocity", this.RB.velocity);
        SDbgPanel.Log("Player Distance", this.DistanceTravelled);

        // death tracking
        if (pos.y < this.Y_KillLine || this.RB.velocity.magnitude < this.KillVelocity)
        {
            EndlessRunnerGameManager.Instance.EndGame();
        }

        // horizontal movement
        pos.x += this._movement * Time.deltaTime * this.SidewaysSpeed;
        this.transform.position = pos;

        // data tracking
        this.DistanceTravelled = this.transform.position.z - this._initialPosition.z;
    }
    #endregion

    #region Helpers
    private void tryMove(InputAction.CallbackContext input)
    {
        this._movement = input.ReadValue<float>();
    }

    private void tryJump()
    {
        foreach (EndlessRunnerPlayerFeet foot in _feetScripts)
        {
            if (foot.CheckGrounded())
            {
                Vector3 vel = this.RB.velocity;
                vel.y = 7f;
                this.RB.velocity = vel;
                return;
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
    #endregion
}

#if UNITY_EDITOR
[CustomEditor(typeof(EndlessRunnerPlayer))]
public class EndlessRunnerPlayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Clean Target"))
        {
            EndlessRunnerPlayer t = (EndlessRunnerPlayer) target;
            Undo.RecordObject(t, "Cleanup EndlessRunnerPlayer");
            t.ForwardSpeed = 5f;
        }
    }
}
#endif
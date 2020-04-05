using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Jet Controller controls the Flying movement
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class SpaceshipController : MonoBehaviour
{
    #region Public Variables
    /// <summary>
    /// Reference to Joystick Controller
    /// </summary>
    [Tooltip("Reference to Joystick Controller")]
    public Joystick joystickController = null;        

    /// <summary>
    /// Forward Speed
    /// </summary>
    [Header("Physics")]
    [Tooltip("Force to push plane forwards with")] public float thrust = 100f;

    /// <summary>
    /// Turning Angle
    /// </summary>
    [Tooltip("Pitch, Yaw, Roll")] public Vector3 turnTorque = new Vector3(90f, 25f, 45f);

    /// <summary>
    /// Force Multiplier
    /// </summary>
    [Tooltip("Multiplier for all forces")] public float forceMult = 1000f;

    #endregion

    #region Private Variables

    /// <summary>
    /// Raycast Radius around the Jet
    /// </summary>
    [SerializeField] private float raycastRadius = 10;

    /// <summary>
    /// Flag to check if Debug Mode is activated
    /// </summary>
    [SerializeField] private bool debugView = false;

    /// <summary>
    /// Player Score
    /// </summary>
    [SerializeField] private int playerScore = 0;

    /// <summary>
    /// Reference to GameManager
    /// </summary>
    [SerializeField] private GameManager gameManager = null;

    /// <summary>
    /// Reference to Rigidbody for Physics
    /// </summary>
    private Rigidbody _rbody;

    /// <summary>
    /// Caching the Transform of Jet
    /// </summary>
    private Transform _transform;

    /// <summary>
    /// Flag to check if Jet can roll Override
    /// </summary>
    private bool _rollOverride = false;

    /// <summary>
    /// Flag to check if Jet can pitch Override
    /// </summary>
    private bool _pitchOverride = false;

    /// <summary>
    /// X axis rotation
    /// </summary>
    private float _pitch = 0f;

    /// <summary>
    /// Y axis rotation
    /// </summary>
    private float _yaw = 0f;

    /// <summary>
    /// Z axis rotation
    /// </summary>
    private float _roll = 0f;

    /// <summary>
    /// Debug Gizmo Color
    /// </summary>
    private Color _gizmoColor;

    #endregion

    /// <summary>
    /// Caching the script reference
    /// </summary>
    private void Awake()
    {
        _rbody = GetComponent<Rigidbody>();

        if (gameManager == null)
        {
            Debug.LogError("Attach reference to GameObject Script");
        }
    }

    /// <summary>
    /// Caching the Transform of Jet
    /// </summary>
    private void Start()
    {
        _transform = transform;

        //  Color For Debug Gizmo
        _gizmoColor = Color.red;
        _gizmoColor.a = 0.2f;
    }

    /// <summary>
    /// Checks for the Joystick Input Controller
    /// </summary>
    private void Update()
    {
        _rollOverride = false;
        _pitchOverride = false;

        float keyboardRoll = joystickController.Horizontal;
        //float keyboardRoll = Input.GetAxis("Horizontal");

        if (Mathf.Abs(keyboardRoll) > .25f)
        {
            _rollOverride = true;
        }

        float keyboardPitch = -joystickController.Vertical;
        //float keyboardPitch = Input.GetAxis("Vertical");

        if (Mathf.Abs(keyboardPitch) > .25f)
        {
            _pitchOverride = true;
            _rollOverride = true;
        }

        // Calculate the smooth stick inputs.
        float autoYaw = 0f;
        float autoPitch = 0f;
        float autoRoll = 0f;

        // Use either keyboard or smooth autopilot input.
        _yaw = autoYaw;
        _pitch = (_pitchOverride) ? keyboardPitch : autoPitch;
        _roll = (_rollOverride) ? keyboardRoll : autoRoll;
    }

    /// <summary>
    /// Appling the Input Controller to Jet using Rigidbody physics
    /// </summary>
    private void FixedUpdate()
    {
        // Ultra simple flight where the plane just gets pushed forward and manipulated
        // with torques to turn.
        _rbody.AddRelativeForce(Vector3.forward * thrust * forceMult, ForceMode.Force);
        _rbody.AddRelativeTorque(new Vector3(turnTorque.x * _pitch,
                                            turnTorque.y * _yaw,
                                            -turnTorque.z * _roll) * forceMult,
                                ForceMode.Force);

        //  Physics Raycast to check the nearby planets
        var raycaster = Physics.OverlapSphere(_transform.position, raycastRadius);
        
        //  Check if it collides with other Planets
        foreach (var collider in raycaster)
        {
            if (collider.CompareTag("Planet"))
            {
                //Vector3 pos = collider.ClosestPointOnBounds(collider.transform.position);

                playerScore++;
                gameManager.SetScore(playerScore);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!debugView)
            return;

        Gizmos.color = _gizmoColor;
        Gizmos.DrawSphere(_transform.position, raycastRadius);
    }

    /// <summary>
    /// If Jet Collides with Planet then destroy the collided Planet
    /// </summary>
    /// <param name="collider">
    /// Planets
    /// </param>
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Planet"))
        {
            //SceneManager.LoadScene("MainMenu");
        }
    }
}

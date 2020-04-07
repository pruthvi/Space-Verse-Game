using UnityEngine;
using UnityEngine.Profiling;
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

    [Header("RayCast Positions")]
    [SerializeField] private Transform centreRayCaster = null;
    [SerializeField] private Transform leftRayCaster = null;
    [SerializeField] private Transform rightRayCaster = null;
    [SerializeField] private Transform forwardRayCaster = null;
    [SerializeField] private Transform backwardRayCaster = null;


    /// <summary>
    /// Reference to Rigidbody for Physics
    /// </summary>
    private Rigidbody _rbody;

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

    private RaycastHit[] _hit;
    private Vector3[] _directions;
    [SerializeField] private float _debugRayTime = 0.5f;

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

        if(!centreRayCaster || !leftRayCaster || !rightRayCaster || !forwardRayCaster || !backwardRayCaster)
        {
            Debug.LogError("Attach the Transform of Raycaster Position First");
        }

        //  Initializing RaycastHit and their directions
        _hit = new RaycastHit[6];
        _directions = new Vector3[]
        {
            Vector3.up, Vector3.left, Vector3.right, Vector3.forward, Vector3.back
        };
    }

    /// <summary>
    /// Caching the Transform of Jet
    /// </summary>
    private void Start()
    {
        //  Color For Debug Gizmo
        _gizmoColor = Color.red;
        _gizmoColor.a = 0.2f;
    }

    /// <summary>
    /// Checks for the Joystick Input Controller
    /// </summary>
    private void Update()
    {
        //  For Testing
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Playground");
        }

        _rollOverride = false;
        _pitchOverride = false;

        //float keyboardRoll = joystickController.Horizontal;
        float keyboardRoll = Input.GetAxis("Horizontal");

        if (Mathf.Abs(keyboardRoll) > .25f)
        {
            _rollOverride = true;
        }

        //float keyboardPitch = -joystickController.Vertical;
        float keyboardPitch = Input.GetAxis("Vertical");

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

        //  Ray casting from Jet
        CastRay(centreRayCaster, _directions[0], _hit[0]);          //  UP
        CastRay(centreRayCaster, -_directions[0], _hit[5]);         //  DOWN

        CastRay(leftRayCaster, _directions[1], _hit[1]);            //  LEFT
        CastRay(rightRayCaster, _directions[2], _hit[2]);           //  RIGHT

        CastRay(forwardRayCaster, _directions[3], _hit[3]);         //  FORWARD
        CastRay(backwardRayCaster, _directions[4], _hit[4]);        //  BACKWARD

    }

    /// <summary>
    /// Shooting a ray
    /// </summary>
    /// <param name="startPos">Ray Starting Position</param>
    /// <param name="directionVector"> Ray shooting Direction</param>
    /// <param name="arrayIndex"></param>
    private void CastRay(Transform startPos, Vector3 directionVector , RaycastHit hit)
    {
        var direction = TransformedDirection(startPos, directionVector);
        if (Physics.Raycast(startPos.position, direction, out hit, raycastRadius))
        {
            if (hit.collider.CompareTag("Planet"))
            {
                playerScore++;
                gameManager.SetScore(playerScore);
                if(debugView)
                    Debug.DrawRay(startPos.position, direction * raycastRadius, Color.red, _debugRayTime);
            }
        }
        if(debugView)
            Debug.DrawRay(startPos.position, direction * raycastRadius, Color.blue);
    }

    /// <summary>
    /// Transform the direction to given direction
    /// </summary>
    /// <param name="currentPos">Current Position</param>
    /// <param name="direction">Direction to Transformed</param>
    /// <returns>Transformed Direction</returns>
    private Vector3 TransformedDirection(Transform currentPos, Vector3 direction)
    {
        return currentPos.TransformDirection(direction);
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

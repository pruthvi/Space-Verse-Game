using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Jet Controller controls the Flying movement
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class JetController : MonoBehaviour
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
    /// Minimum Score player can make when jet is nearby planet
    /// </summary>
    [Tooltip("Minimum Score player can make when jet is nearby planet")]
    [SerializeField] private int minPlayerScore = 0;

    /// <summary>
    /// Screen Overlay Score
    /// </summary>
    [SerializeField] private Text overlayScore;

    /// <summary>
    /// Screen Overlay Score
    /// </summary>
    [SerializeField] private Text debugTestingText;

    /// <summary>
    /// Reference to GameManager
    /// </summary>
    [SerializeField] private GameManager gameManager = null;

    [SerializeField] private ResetPlayer resetPlayer = null;

    [Header("RayCast Positions")]
    [SerializeField] private Transform centreRayCaster = null;
    [SerializeField] private Transform leftRayCaster = null;
    [SerializeField] private Transform rightRayCaster = null;
    [SerializeField] private Transform forwardRayCaster = null;
    [SerializeField] private Transform backwardRayCaster = null;

    /// <summary>
    /// Caching Jet Transform
    /// </summary>
    private Transform _transform;

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
    /// Player Score
    /// </summary>
    private int _playerScore = 0;

    /// <summary>
    /// Debug Gizmo Color
    /// </summary>
    private Color _gizmoColor;

    /// <summary>
    /// Array of colliders to store the information of collided objects
    /// </summary>
    private Collider[] _collider;

    /// <summary>
    /// Raycast hit for storing the information of collided ray to the object
    /// </summary>
    private RaycastHit[] _hit;

    /// <summary>
    /// Array of different Directions to shot the ray towards
    /// </summary>
    private Vector3[] _directions;

    /// <summary>
    /// Debugging Ray display time
    /// </summary>
    [SerializeField] private float _debugRayTime = 0.5f;

    /// <summary>
    /// Flag to check if score is displaying on screen
    /// </summary>
    private bool _isScoreDisplaying = false;

    /// <summary>
    /// Flag to check if score is reseted
    /// </summary>
    private bool _scoreReseted = false;

    /// <summary>
    /// Score timer to track the elapsed time
    /// </summary>
    private float _scoreTimer = 0;

    /// <summary>
    /// After 2 seconds, overlay score will be hided
    /// </summary>
    private int _timeToHideScore = 2;     

    /// <summary>
    /// Flag to set the values for starting the score hiding timer
    /// </summary>
    private bool StartScoreTimer {
        set
        {
            _isScoreDisplaying = value;
            _scoreTimer = 0;
        } 
    }
    
    #endregion

    /// <summary>
    /// Caching the script reference
    /// </summary>
    private void Awake()
    {
        _transform = transform;
        _rbody = GetComponent<Rigidbody>();

        if (!gameManager || !resetPlayer)
        {
            Debug.LogError("Attach reference to GameObject Script");
        }

        if(!centreRayCaster || !leftRayCaster || !rightRayCaster || !forwardRayCaster || !backwardRayCaster)
        {
            Debug.LogError("Attach the Transform of Raycaster Position First");
        }

        //  Initializing RaycastHit and their directions
        //_collider = new Collider[5];
        _hit = new RaycastHit[10];
        _directions = new Vector3[]
        {
            Vector3.up,
            -Vector3.up,
            Vector3.left,
            Vector3.right,
            Vector3.forward,
            Vector3.back,
            (Vector3.forward + Vector3.right).normalized,
            (Vector3.forward + Vector3.left).normalized,
            (Vector3.back + Vector3.right).normalized,
            (Vector3.back + Vector3.left).normalized
        };

        //  Color For Debug Gizmo
        _gizmoColor = Color.red;
        _gizmoColor.a = 0.2f;

        overlayScore.text = " ";
        debugTestingText.text = " ";

        //ResetPlayer();
    }

    /// <summary>
    /// Checks for the Joystick Input Controller
    /// </summary>
    private void Update()
    {
        //  For Testing
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResetPlayer();
        }

        //  Holding Shift key will increase thrust
        thrust = Input.GetKey(KeyCode.LeftShift) ? 600 : 200;


        #region _____JET MOVEMENT CONTROLLER_____
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

        #endregion

        #region _____OVERLAY SCORE HIDING TIMER_____

        //  Timer to hide Overlay Score UI
        if (_isScoreDisplaying)             //  If the Score is currently being displaying then start timer
        {
            if (_scoreTimer < _timeToHideScore)
            {
                _scoreTimer += Time.deltaTime;
            }
            else                            //  When Timer has passed the Time Limit, Reset the flags
            {
                _isScoreDisplaying = false;
                _scoreReseted = false;
            }
        }

        if (!_isScoreDisplaying)            //  Reset the OverlayScore and hide it.
        {
            if (!_scoreReseted)
            {
                gameManager.SetScore(_playerScore);
                _scoreReseted = true;
                _playerScore = 0;
                overlayScore.text = " ";    //  TODO: Hide with Animation
            }
        }

        #endregion
    }
     
    /// <summary>
    /// Applying the Input Controller to Jet using Rigidbody physics
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
        CastRay(centreRayCaster, _directions[1], _hit[1]);         //  DOWN
        
        CastRay(leftRayCaster, _directions[2], _hit[2]);            //  LEFT
        CastRay(rightRayCaster, _directions[3], _hit[3]);           //  RIGHT

        CastRay(forwardRayCaster, _directions[4], _hit[4]);         //  FORWARD
        CastRay(backwardRayCaster, _directions[5], _hit[5]);        //  BACKWARD

        CastRay(centreRayCaster, _directions[6], _hit[6]);          //  45 FORWARD RIGHT
        CastRay(centreRayCaster, _directions[7], _hit[7]);          //  45 FORWARD LEFT

        CastRay(centreRayCaster, _directions[8], _hit[8]);          //  45 BACKWARD RIGHT
        CastRay(centreRayCaster, _directions[9], _hit[9]);          //  45 BACKWARD LEFT
        
        /*
        var result = Physics.OverlapSphereNonAlloc(centreRayCaster.position, raycastRadius, _collider);
        for (int i = 0; i < result; i++)
        {
            if(_collider[i].CompareTag("Planet"))
            {
                Debug.DrawLine(centreRayCaster.position, _collider[i].transform.position, Color.red);
                //_playerScore += minPlayerScore + (int)(1 / _collider[i].distance);
                _playerScore += minPlayerScore;
                overlayScore.text = _playerScore.ToString();


                StartScoreTimer = true;
            }
        }*/
        
    }


    /// <summary>
    /// Shooting a ray
    /// </summary>
    /// <param name="startPos">Ray Starting Position</param>
    /// <param name="directionVector"> Ray shooting Direction</param>
    /// <param name="hit">Raycast Hit details</param>
    private void CastRay(Transform startPos, Vector3 directionVector , RaycastHit hit)
    {
        var direction = TransformedDirection(startPos, directionVector);
        if (Physics.Raycast(startPos.position, direction, out hit, raycastRadius))
        {
            if (hit.collider.CompareTag("Planet"))
            {
                _playerScore += minPlayerScore + (int)(1 / hit.distance);

                //  Start Overlay Score Timer
                overlayScore.text = _playerScore.ToString();

                StartScoreTimer = true;

                if (debugView)
                    Debug.DrawRay(startPos.position, direction * raycastRadius, Color.red, _debugRayTime);
            }
        }
        if (debugView)
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
    /// Resetting Jet to its starting position
    /// </summary>
    public void ResetPlayer()
    {
        overlayScore.text = " ";
        debugTestingText.text = "Gone Out of Range, so Forced Resetted position ";
        StartCoroutine(Wait3Seconds());

    }

    /// <summary>
    /// Wait for 3 seconds and restart the player
    /// </summary>
    /// <returns></returns>
    private IEnumerator Wait3Seconds()
    {
        //  TODO: Display Foggy animation on screen to fade out Jet

        yield return new WaitForSeconds(3);
        debugTestingText.text = " ";

        resetPlayer.RestartPlayer();
    }

    /// <summary>
    /// Died Screen / GameOver
    /// </summary>
    private void RestartGame()
    {
        overlayScore.text = " ";
        //  Play Jet Crashing animation

        //  Show Player Die Screen
        resetPlayer.PlayerDiedScreen();
    }

    /// <summary>
    /// If Jet Collides with Planet then destroy the collided Planet
    /// </summary>
    /// <param name="other">
    /// Planets
    /// </param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Planet"))
        {
            Debug.Log("KABOOOOM!!....Collided with the planet");
            RestartGame();
            //SceneManager.LoadScene("MainMenu");
        }
    }
}

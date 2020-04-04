using UnityEngine;

/// <summary>
/// SpaceShip Controller controls the Flying movement
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
    /// Reference to Rigidbody for Physics
    /// </summary>
    private Rigidbody rigid;

    /// <summary>
    /// Flag to check if Spaceship can roll Override
    /// </summary>
    private bool rollOverride = false;

    /// <summary>
    /// Flag to check if Spaceship can pitch Override
    /// </summary>
    private bool pitchOverride = false;

    /// <summary>
    /// X axis rotation
    /// </summary>
    private float pitch = 0f;

    /// <summary>
    /// Y axis rotation
    /// </summary>
    private float yaw = 0f;

    /// <summary>
    /// Z axis rotation
    /// </summary>
    private float roll = 0f;

    #endregion

    /// <summary>
    /// Caching the script reference
    /// </summary>
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Checks for the Joystick Input Controller
    /// </summary>
    private void Update()
    {
        rollOverride = false;
        pitchOverride = false;

        float keyboardRoll = joystickController.Horizontal;
        if (Mathf.Abs(keyboardRoll) > .25f)
        {
            rollOverride = true;
        }

        float keyboardPitch = joystickController.Vertical;
        if (Mathf.Abs(keyboardPitch) > .25f)
        {
            pitchOverride = true;
            rollOverride = true;
        }

        // Calculate the smooth stick inputs.
        float autoYaw = 0f;
        float autoPitch = 0f;
        float autoRoll = 0f;

        // Use either keyboard or smooth autopilot input.
        yaw = autoYaw;
        pitch = (pitchOverride) ? keyboardPitch : autoPitch;
        roll = (rollOverride) ? keyboardRoll : autoRoll;
    }

    /// <summary>
    /// Appling the Input Controller to SpaceShip using Rigidbody physics
    /// </summary>
    private void FixedUpdate()
    {
        // Ultra simple flight where the plane just gets pushed forward and manipulated
        // with torques to turn.
        rigid.AddRelativeForce(Vector3.forward * thrust * forceMult, ForceMode.Force);
        rigid.AddRelativeTorque(new Vector3(turnTorque.x * pitch,
                                            turnTorque.y * yaw,
                                            -turnTorque.z * roll) * forceMult,
                                ForceMode.Force);
    }

    /// <summary>
    /// If SpaceShip Collides with Planet then destroy the collided Planet
    /// </summary>
    /// <param name="collider">
    /// Planets
    /// </param>
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Planet"))
        {
            Destroy(collider.gameObject);
        }
    }
}

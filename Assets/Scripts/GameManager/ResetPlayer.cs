using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

/// <summary>
/// Reset Player and Restart the game
/// </summary>
public class ResetPlayer : MonoBehaviour
{
    [SerializeField] private GameManager gameManager = null;        //  Reference to GameManager
    [SerializeField] private GameObject jetGameObject;              //  Reference to Jet GameObject
    [SerializeField] private JetController jetController;           //  Reference to JetController Script
    [SerializeField] private Animator playerRestartAnim;            //  Player Restarting Animator
    [SerializeField] private Joystick joystickController;           //  Joystick controller
    [SerializeField] private CinemachineVirtualCamera cam;          //  Cinemachine Camera

    private Transform _jetTransform;                                        //  Transform of Jet GameObject
    private Transform _camTarget;                                           //  Camera Follow/LookAt Target
    private Vector3 _camPosition;                                           //  Initial Camera Position
    private Quaternion _camRotation;                                        //  Initial Camera rotation
    private GameObject _joystickGameObject;                                 //  Joystick Controller Gameobject
    private int playerDozeIn = Animator.StringToHash("PlayerDozeIn");   //  Hashing Animator string to int
    private int resetCam = Animator.StringToHash("Reset");              //  Hashing Animator string to int

    // Caching the required components
    private void Awake()
    {
        _camPosition = new Vector3(0,0,-150);
        _camRotation = new Quaternion(0,0,0,0);

        //  Storing camera's following target
        _camTarget = cam.m_Follow;      

        _jetTransform = jetGameObject.transform;
        _joystickGameObject = joystickController.gameObject;
       
    }

    /// <summary>
    /// Restarting Player(Jet) with some camera effect
    /// </summary>
    public void RestartPlayer()
    {
        DisablePlayerMovement();

        StartCoroutine(WaitForCameraEffect(0.5f));
    }

    /// <summary>
    /// Restart Game by Initializing everything
    /// </summary>
    public void RestartGame()
    {
        jetController.DarkEffectFadeOut(100f);
        gameManager.InitializeGame();
        RestartPlayer();
    }

    /// <summary>
    /// Player Died Screen
    /// </summary>
    public void PlayerDiedScreen()
    {
        DisablePlayerMovement();

        //  Show scoring Count animation
        gameManager.DisplayTotalScore();
    }

    /// <summary>
    /// Start Camera effect
    /// </summary>
    /// <param name="waitTime">Wait for 0.5f seconds</param>
    /// <returns></returns>
    private IEnumerator WaitForCameraEffect(float waitTime)
    {
        //  Reset Everything
        jetGameObject.SetActive(false);
        ResetCamera();
        DisablePlayerMovement();

        //  Play Camera animation
        playerRestartAnim.SetTrigger(playerDozeIn);

        yield return new WaitForSeconds(waitTime);

        joystickController.ResetInput();
        _jetTransform.position = Vector3.zero;
        _jetTransform.rotation = Quaternion.identity;

        //  Init with updated changes
        jetGameObject.SetActive(true);
        UpdateCameraSetting();
        
        //  Disable Overlay Dark Effect
        jetController.DarkEffectFadeIn(1);

        EnablePlayerMovement();

        playerRestartAnim.SetTrigger(resetCam);
    }

    /// <summary>
    /// Enable Player Movement
    /// </summary>
    private void EnablePlayerMovement()
    {
        //  Enable Jet Movement Controller
        jetController.enabled = true;

        //  Enable Joystick
        _joystickGameObject.SetActive(true);
        //jetController.enableMovement = true;
    }

    /// <summary>
    /// Disable Player Movement
    /// </summary>
    private void DisablePlayerMovement()
    {
        //  Disable Joystick
        _joystickGameObject.SetActive(false);

        //  Disable Jet Movement Controller
        jetController.enabled = false;
    }

    /// <summary>
    /// Reset camera settings
    /// </summary>
    private void ResetCamera()
    {
        cam.m_LookAt = null;
        cam.transform.position = _camPosition;
        cam.transform.rotation = _camRotation;
    }

    /// <summary>
    /// Update Camera lookAt settings
    /// </summary>
    private void UpdateCameraSetting()
    {
        cam.m_LookAt = _camTarget;
    }
}

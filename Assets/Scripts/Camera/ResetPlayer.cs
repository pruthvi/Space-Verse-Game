using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ResetPlayer : MonoBehaviour
{
    public float findTime = 1.5f;
    /// <summary>
    /// Reference to GameManager
    /// </summary>
    [SerializeField] private GameManager gameManager = null;

    [SerializeField] private GameObject jetGameObject;
    [SerializeField] private SpaceshipController jetController;
    [SerializeField] private Animator playerRestartAnim;
    [SerializeField] private Joystick joystickController;
    [SerializeField] private CinemachineVirtualCamera cam;

    private Transform _jetTransform;    //  Transform of Jet GameObject
    private Transform _camTarget;   //  Camera Follow/LookAt Target
    private Vector3 _camPosition;   //  Initial Camera Position
    private Quaternion _camRotation;    //  Initial Camera rotation
    private GameObject _joystickGameObject;
    private int playerDozeIn = Animator.StringToHash("PlayerDozeIn");
    private int resetCam = Animator.StringToHash("Reset");

    // Start is called before the first frame update
    private void Awake()
    {
        if (!gameManager)
            gameManager = GetComponent<GameManager>();

        _camPosition = new Vector3(0,0,-150);

        _camRotation = new Quaternion(0,0,0,0);
        _camTarget = cam.m_Follow;      //  Storing camera's following target

        _jetTransform = jetGameObject.transform;
        _joystickGameObject = joystickController.gameObject;
       
    }

    public void RestartPlayer()
    {
        cam.gameObject.SetActive(true);
        DisableMovement();
        gameManager.GameStarted();
        StartCoroutine(WaitForCameraEffect(2));

    }

    private void EnableMovement()
    {
        //  Enable Jet Movement Controller
        jetController.enabled = true;

        

        //  Enable Joystick
        _joystickGameObject.SetActive(true);
        jetController.enableMovement = true;

    }

    private void DisableMovement()
    {
        //  Disable Joystick
        _joystickGameObject.SetActive(false);

        //  Disable Jet Movement Controller
        jetController.enabled = false;
    }

    private void DisableCamera()
    {
        //cam.m_Follow = null;
        cam.m_LookAt = null;
        cam.transform.position = _camPosition;
        cam.transform.rotation = _camRotation;
    }

    private void EnableCamera()
    {
        //cam.m_Follow = _camTarget;
        cam.m_LookAt = _camTarget;
    }

    public void PlayerDieScreen()
    {
        DisableMovement();

        //  Show scoring Count animation
        gameManager.DisplayTotalScore();

    }

    private IEnumerator WaitForCameraEffect(float waitTime)
    {
        jetGameObject.SetActive(false);
        DisableCamera();
        DisableMovement();

        playerRestartAnim.SetTrigger(playerDozeIn);

        yield return new WaitForSeconds(findTime);     // TODO : Find Exact Time || Wait 1 second to finish the Camera Animation

        joystickController.ResetInput();
        _jetTransform.position = Vector3.zero;
        _jetTransform.rotation = Quaternion.identity;

        jetGameObject.SetActive(true);
        EnableCamera();

        EnableMovement();

        //yield return new WaitForSeconds(waitTime);

        playerRestartAnim.SetTrigger(resetCam);

    }
}

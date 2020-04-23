using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class GeneratePlanet : MonoBehaviour
{
    #region Public Variables

    /// <summary>
    /// Planet GameObject to Spawn
    /// </summary>
    public GameObject planet = null;

    /// <summary>
    /// Total Number of Planets to Spawn
    /// </summary>
    public int noOfPlanets = 200;

    /// <summary>
    /// Range to Generate Planets
    /// </summary>
    public float distToGen = 50.0f;

    /// <summary>
    /// Planet Minimum Size
    /// </summary>
    public float minPlanetSize = 2.0f;

    /// <summary>
    /// Planet Maximum Size
    /// </summary>
    public float maxPlanetSize = 10.0f;

    /// <summary>
    /// Get the Color Palette from the List of PreDefined colors
    /// </summary>
    public ColorPaletteGenerator colorPalette;
    
    /// <summary>
    /// Random Color Palette
    /// </summary>
    [HideInInspector] public Color[] _colors;

    #endregion

    #region Private Variables

    /// <summary>
    /// Cached Transform property
    /// </summary>
    private Transform _transform;
    
    /// <summary>
    /// Player Controller
    /// </summary>
    [SerializeField]private JetController _playerController;

    /// <summary>
    /// Caching GameObject Position
    /// </summary>
    private Vector3 _position;

    /// <summary>
    /// Overlap Circular Radius Offset
    /// </summary>
    private float _overlapRadiusOffset = 5.0f;

    /// <summary>
    /// New Generated Planet Position
    /// </summary>
    private Vector3 _planetPos;

    /// <summary>
    /// New Planet's Radius
    /// </summary>
    private float _newPlanetRadius;

    /// <summary>
    /// Retry attempt if planet overlaps on each other
    /// </summary>
    private int _maxSpawnAttempt = 5;

    /// <summary>
    /// Game Boundry to bound the player in the game
    /// </summary>
    private SphereCollider _gameBoundary = null;

    /// <summary>
    /// Extra offset to the boundary for smooth effect
    /// </summary>
    private int _offsetBoundary = 20;

    /// <summary>
    /// Color Palette array length
    /// </summary>
    private int _colorsLength = 0;

    #endregion

    /// <summary>
    /// Caching required Components
    /// </summary>
    private void Awake()
    {
        _position = transform.position;
        _transform = transform;
        _gameBoundary = GetComponent<SphereCollider>();
        if (!_playerController)
            _playerController = FindObjectOfType<JetController>();
    }

    /// <summary>
    /// Initializing Game Boundary and Starting Creating Galaxy
    /// </summary>
    private void Start()
    {
        _gameBoundary.radius = distToGen + _offsetBoundary;

        //  Get the Random Color Palette
        _colors = colorPalette.GetColor(Random.Range(0,colorPalette.colorPalettes.Length));
        _colorsLength = _colors.Length;

        DestroyGalaxy();

        //  Create Galaxy on starting up the scene
        StartCoroutine(CreateGalaxy());
    }


    /// <summary>
    /// Create Galaxy with the different size planets
    /// </summary>
    /// <returns></returns>
    private IEnumerator CreateGalaxy()
    {
        //  Loop around and Generate Random Planets at random Position
        for (int i = 0; i < noOfPlanets; i++)
        {
            //  Check if we can Spawn
            bool isValidPosition = false;
            //  Number of Planet spawned
            int planetCount = 0;

            while (!isValidPosition && planetCount < noOfPlanets)
            {
                //  Increase planet Counter
                planetCount++;

                var pos = GenerateRandomPos();
                _newPlanetRadius = GenerateRandomRadius(minPlanetSize, maxPlanetSize);
                _planetPos = pos;

                //  This position is valid until proven invalid
                isValidPosition = true;

                //  Collecting all colliders within our planet radius check
                Collider[] colliders = Physics.OverlapSphere(_planetPos, _newPlanetRadius + _overlapRadiusOffset);

                //  Check if it collides with other Planets
                foreach (var planetCollider in colliders)
                {
                    if (planetCollider.CompareTag("Planet"))
                    {
                        //  Spawn position is not valid
                        isValidPosition = false;
                    }
                }
            }

            //  If It has valid Position then Spawn Planet
            if (isValidPosition)
            {
                var newPlanet = Instantiate(planet, _planetPos, Quaternion.identity, gameObject.transform);
                newPlanet.transform.localScale *= _newPlanetRadius;
                
                //  Apply Material  
                var randomColor = Random.Range(0, _colorsLength);
                var renderer = newPlanet.GetComponent<MeshRenderer>();
                renderer.material.SetColor("_Color", _colors[randomColor]);
            }

            //  [Optional] Spawning Planet one by one by giving certain time limit between spawn
            yield return new WaitForSeconds(0.000001f);
        }
    }

    /// <summary>
    /// Generate Random Number in Range
    /// </summary>
    /// <param name="min">Minimum radius value</param>
    /// <param name="max">Maximum radius value</param>
    /// <returns>Returns random number from range</returns>
    private float GenerateRandomRadius(float min, float max)
    {
        return Random.Range(min, max);
    }

    /// <summary>
    /// Generate Random Planet Position inside Distance Radius
    /// </summary>
    /// <returns>Returns random position inside the dimension</returns>
    private Vector3 GenerateRandomPos()
    {
        return (Random.insideUnitSphere * distToGen) + _position;
    }

    /// <summary>
    /// Clears all the generated Planets
    /// </summary>
    public void DestroyGalaxy()
    {
        foreach (Transform planet in _transform)
        {
            GameObject.Destroy(planet.gameObject);
        }
    }

    /// <summary>
    /// Check when player enters in the Game Boundary
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag((_playerController.tag)))
        {
            Debug.Log("Player Enter the Game Boundary");
        }
    }

    /// <summary>
    /// Check when player Exits the Game Boundary
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag((_playerController.tag)))
        {
            _playerController.ResetPlayer();
            Debug.Log("Player Exited the Game Boundary");
        }
    }

    /// <summary>
    /// When this script is Destroyed
    /// </summary>
    private void OnDestroy()
    {
        DestroyGalaxy();
    }
}

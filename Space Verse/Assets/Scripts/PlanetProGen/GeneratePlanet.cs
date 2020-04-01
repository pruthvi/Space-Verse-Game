using System.Collections;
using UnityEngine;

public class GeneratePlanet : MonoBehaviour
{
    //  Public Variables
    public GameObject planet = null;        //  Planet GameObject to Spawn
    public int noOfPlanets = 200;           //  Total Number of Planets to Spawn
    public float distToGen = 50.0f;         //  Range to Generate Planets
    public float minPlanetSize = 2.0f;      //  Planet Minimum Size
    public float maxPlanetSize = 10.0f;     //  Planet Maximum Size

    //  Private Variables
    private float _overlapRadiusOffset = 5.0f;      //  Overlap Circular Radius Offset
    private Vector3 _planetPos;                     //  New Generated Planet Position
    private float _newPlanetRadius;                 //  New Planet's Radius
    private int _maxSpawnAttempt = 5;               //  Retry attempt if planet overlaps on each other

    private void Start()
    {
        //  Create Galaxy on starting up the scene
        StartCoroutine(CreateGalaxy());
    }

    //  Create Galaxy with the different size planets
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
                _newPlanetRadius = GenerateRandom(minPlanetSize, maxPlanetSize);

                _planetPos = pos;

                //  This position is valid until proven invalid
                isValidPosition = true;

                //  Collecting all colliders within our planet radius check
                Collider[] colliders = Physics.OverlapSphere(_planetPos, _newPlanetRadius + _overlapRadiusOffset);

                //  Check if it collides with other memories
                foreach (var collider in colliders)
                {
                    if (collider.CompareTag("Planet"))
                    {
                        //  Spawn position is not valid
                        isValidPosition = false;
                    }
                }
            }

            //  If It has valid Position then Spawn Planet
            if (isValidPosition)
            {
                var newPlanet = Instantiate(planet, _planetPos, Quaternion.identity);
                newPlanet.transform.localScale *= _newPlanetRadius;
                newPlanet.transform.parent = gameObject.transform;
            }

            //  [Optional] Spawning Planet one by one by giving certain time limit between spawn
            yield return new WaitForSeconds(0.01f);
        }

    }

    //  Generate Random Number in Range
    private float GenerateRandom(float min, float max)
    {
        return Random.Range(min, max);
    }

    //  Generate Random Planet Position inside Distance Radius
    private Vector3 GenerateRandomPos()
    {
        float x = GenerateRandom(-distToGen, distToGen);
        float y = GenerateRandom(-distToGen, distToGen);
        float z = GenerateRandom(0, distToGen);
        Vector3 pos = new Vector3(x, y, z);
        return Random.insideUnitSphere * distToGen + pos;

    }
}

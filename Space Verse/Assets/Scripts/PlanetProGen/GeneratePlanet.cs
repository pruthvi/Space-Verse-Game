using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePlanet : MonoBehaviour
{

    public GameObject Planet;
    public int NoOfPlanets;
    public float distToGen;
    int TotalPlanet;
    bool initGalaxy = false;
    float timer = 0;

    void Start()
    {
        CreateGalaxy(NoOfPlanets);
        // TotalPlanet = 0;
        initGalaxy = false;
    }

    void FixedUpdate()
    {
        timer = timer + Time.deltaTime;
        if (timer < 5)
        {
            Debug.Log("Time: " + timer);

            TotalPlanet = GameObject.FindGameObjectsWithTag("Planet").Length;
            Debug.Log("Total Planets" + TotalPlanet + " || " + initGalaxy);

            if (/*initGalaxy == true && */ TotalPlanet < NoOfPlanets)
            {
                GenPlanet();
            }

        }


        // while (TotalPlanet <= NoOfPlanets)
        // {
        //     GenPlanet();
        //     TotalPlanet++;
        // }
    }

    void CreateGalaxy(int no)
    {
        for (int i = 0; i <= no; i++)
        {
            GenPlanet();
        }
        initGalaxy = true;

    }

    float GetRandomFloat(float min, float max)
    {
        float value = Random.Range(min, max);
        return value;
    }

    public void GenPlanet()
    {
        float planetSize = GetRandomFloat(0.5f, 5f);
        Vector3 pSize = new Vector3(planetSize, planetSize, planetSize);

        float x = GetRandomFloat(-distToGen, distToGen);
        float y = GetRandomFloat(-distToGen, distToGen);
        float z = GetRandomFloat(0, distToGen);
        Vector3 pos = new Vector3(x, y, z);

        GameObject p = Instantiate(Planet, pos, Quaternion.identity);
        p.transform.localScale = pSize;
        p.transform.parent = gameObject.transform;

    }
}

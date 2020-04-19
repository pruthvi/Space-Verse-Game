using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkyboxEditor : MonoBehaviour
{
    private Skybox _skybox;

    [SerializeField] private GeneratePlanet planetManager;

    public Action colorGenerated;

    private void Awake()
    {
        _skybox = GetComponent<Skybox>();

        if (!planetManager)
        {
            planetManager = FindObjectOfType<GeneratePlanet>();
        }
    }

    private void Start()
    {
        //colorGenerated += ApplySkyboxColor;
        ApplySkyboxColor();
        //ApplySkyboxColor(planetManager.hueMin, planetManager.hueMax);
    }

    /*public void ApplySkyboxColor(float hueMin, float hueMax)
    {
        var randomColor = Random.ColorHSV(hueMin, hueMax, 1,1,1,1);
        _skybox.material.SetColor("_Tint", randomColor);
    }*/

    public void ApplySkyboxColor()
    {
        var color = planetManager._colors[0];
        _skybox.material.SetColor("_Tint", color);
    }
}

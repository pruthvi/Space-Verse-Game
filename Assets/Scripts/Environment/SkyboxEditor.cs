using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkyboxEditor : MonoBehaviour
{
    private Skybox _skybox;

    [SerializeField] private GeneratePlanet planetManager;

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
        ApplySkyboxColor();
    }

    /// <summary>
    /// Apply Skybox Color from the color palette
    /// </summary>
    public void ApplySkyboxColor()
    {
        //  Get the first color from the Color Palette
        var color = planetManager._colors[0];
        _skybox.material.SetColor("_Tint", color);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        ApplySkyboxColor(planetManager.hueMin, planetManager.hueMax);
    }

    private void ApplySkyboxColor(float hueMin, float hueMax)
    {
        var randomColor = Random.ColorHSV(hueMin, hueMax, 1,1,1,1);
        _skybox.material.SetColor("_Tint", randomColor);
        RenderSettings.fogColor = randomColor;
    }
}

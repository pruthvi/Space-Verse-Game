using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    private MeshRenderer _renderer;     //  Planet's mesh Renderer

    public float hueMin = 0.5f;
    public float hueMax = 0.75f;

    //  Cache reference
    private void Awake()
    {
        //_renderer = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        //GenerateRandomColor();
    }

    //  Generate Random Color and Apply to the Planet's Material
    private void GenerateRandomColor()
    {
        Color randomColor = Random.ColorHSV(hueMin, hueMax, 0.5f, 0.5f, 1, 1);

        Material mat = new Material(Shader.Find("Unlit/Color"));
        mat.SetColor("_Color", randomColor);
        //mat.EnableKeyword("_EMISSION");
        //mat.SetColor("_EmissionColor", randomColor);
        _renderer.material = mat;
    }
}

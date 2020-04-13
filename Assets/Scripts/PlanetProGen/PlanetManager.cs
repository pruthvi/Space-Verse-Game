using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    private MeshRenderer _renderer;     //  Planet's mesh Renderer

    //  Cache reference
    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        GenerateRandomColor();
    }

    //  Generate Random Color and Apply to the Planet's Material
    private void GenerateRandomColor()
    {
        Color randomColor = Random.ColorHSV(0,1,0.5f,1,1,1);

        Material mat = new Material(Shader.Find("Standard"));
        mat.SetColor("_Color", randomColor);
        _renderer.material = mat;
    }
}

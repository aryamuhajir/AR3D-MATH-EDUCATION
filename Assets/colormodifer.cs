using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeColorModifier : MonoBehaviour
{
    public MeshRenderer cube;
    public Slider red;
    public Slider green;
    public Slider blue;

    public void OnEdit()
    {
        Color color = cube.material.color;
        color.r = red.value;
        color.g = green.value;
        color.b = blue.value;
        cube.material.color = color;
        cube.material.SetColor("_EmissionColor", color);
    }
}
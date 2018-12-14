using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class warnabalok : MonoBehaviour {
    public MeshRenderer cube1;
    public MeshRenderer cube2;
    public MeshRenderer cube3;
    public Slider red;
 

    public void OnEdit ()
    {
        Color color1 = cube1.material.color;
        Color color2 = cube2.material.color;
        Color color3 = cube3.material.color;
        color1.a = red.value;
        color2.a = red.value;
        color3.a = red.value;
        cube1.material.color = color1;
        cube1.material.SetColor("_EmissionColor", color1);
        cube2.material.color = color2;
        cube2.material.SetColor("_EmissionColor", color2);
        cube3.material.color = color3;
        cube3.material.SetColor("_EmissionColor", color3);

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectrotate : MonoBehaviour {

    public GameObject objectRotate;
    public GameObject objectRotate2;
    public GameObject objectRotate3;

    public float rotateSpeed = 50f;
    bool rotateStatus = false;

    public void Rotasi()
    {

        if (rotateStatus == false)
        {
            rotateStatus = true;
        }
        else
        {
            rotateStatus = false;
        }
    }

    void Update()
    {
        if (rotateStatus == true)
        {
            objectRotate.transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
            objectRotate2.transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
            objectRotate3.transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
        }
    }
}
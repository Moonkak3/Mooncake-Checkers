using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotationSpeed;
    void Update()
    {
        if (gameObject.activeSelf)
        {
            transform.Rotate(Vector3.right * (rotationSpeed * Time.deltaTime));
        }
    }
}

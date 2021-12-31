using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMovement : MonoBehaviour
{
    public bool moving = false;
    public void MoveTo(Vector3 endCoords, float time)
    {
        StartCoroutine(Movement.MoveTo(gameObject, transform.localPosition, endCoords, time));
    }
}

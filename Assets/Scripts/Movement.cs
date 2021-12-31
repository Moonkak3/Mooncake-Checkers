using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Movement
{
    public static IEnumerator MoveTo(GameObject go, Vector3 startCoords, Vector3 endCoords, float time)
    {
        go.GetComponent<SmoothMovement>().moving = true;
        float currentTime = 0;
        while (Vector3.Distance(go.transform.localPosition, endCoords) > 0)
        {
            currentTime += Time.deltaTime;
            go.transform.localPosition = Vector3.Lerp(startCoords, endCoords, currentTime / time);
            yield return null;
        }
        go.GetComponent<SmoothMovement>().moving = false;
    }
}

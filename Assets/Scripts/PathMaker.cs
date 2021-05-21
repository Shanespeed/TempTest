using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMaker : MonoBehaviour
{
    [HideInInspector]
    public static List<Vector3> pathPositions = new List<Vector3>();

    // Calculate the path directly
    void Awake() =>
        CalculatePath();

    private void CalculatePath()
    {
        //Gets all child objects
        foreach (Transform child in transform)
        {
            //Add all positions from the children for a position array
            pathPositions.Add(child.position);

            //Draws a visible path for debugging
            if (pathPositions.Count > 1)
                Debug.DrawLine(pathPositions[pathPositions.Count - 1], pathPositions[pathPositions.Count - 2], Color.red, 5f);
        }
    }

}

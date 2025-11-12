using UnityEngine;

public class PathPoint : MonoBehaviour
{
    public bool isCrossing = true; // Tick this in Inspector for crossroads
    public PathPoint[] nextOptions; // assign next possible points in Inspector
}
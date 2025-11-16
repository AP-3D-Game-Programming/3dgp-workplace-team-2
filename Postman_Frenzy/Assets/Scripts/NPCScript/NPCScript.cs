using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCScript : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject PATH;
    public Transform[] PathPoints;

    public float minDistance = 5f;
    public int index = 0;
    public int direction = 1; // 1 = forward, -1 = backward

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Get all path points
        PathPoints = new Transform[PATH.transform.childCount];
        for (int i = 0; i < PathPoints.Length; i++)
        {
            PathPoints[i] = PATH.transform.GetChild(i);
        }

        // Find the closest point to start
        float closestDist = Mathf.Infinity;
        for (int i = 0; i < PathPoints.Length; i++)
        {
            float dist = Vector3.Distance(transform.position, PathPoints[i].position);
            if (dist < closestDist)
            {
                closestDist = dist;
                index = i;
            }
        }

        agent.SetDestination(PathPoints[index].position);
    }

    void Update()
    {
        Roam();
    }

    void Roam()
    {
        if (Vector3.Distance(transform.position, PathPoints[index].position) < minDistance)
        {
            // Rarely change direction (5% chance)
            if (Random.value < 0.05f)
            {
                direction *= -1;
            }

            // Move to next point in the chosen direction
            index += direction;

            // Wrap around if we reach the ends
            if (index >= PathPoints.Length)
                index = 0;
            else if (index < 0)
                index = PathPoints.Length - 1;

            agent.SetDestination(PathPoints[index].position);
        }
    }
}

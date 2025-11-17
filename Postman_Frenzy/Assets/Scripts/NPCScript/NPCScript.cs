using System.Collections;
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

    IEnumerator Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Gather path points
        int count = PATH.transform.childCount;
        PathPoints = new Transform[count];
        for (int i = 0; i < count; i++)
        {
            PathPoints[i] = PATH.transform.GetChild(i);
        }

        // Find closest starting point
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

        // Initial move
        agent.SetDestination(PathPoints[index].position);

        // NPC "thinks" every 0.3 seconds instead of every frame
        while (true)
        {
            Roam();
            yield return new WaitForSeconds(0.3f);
        }
    }

    void Roam()
    {
        if (Vector3.Distance(transform.position, PathPoints[index].position) < minDistance)
        {
            // 5% chance to reverse
            if (Random.value < 0.05f)
            {
                direction *= -1;
            }

            // Move along
            index += direction;

            // Wrap around
            if (index >= PathPoints.Length)
                index = 0;
            else if (index < 0)
                index = PathPoints.Length - 1;

            // Set next destination
            agent.SetDestination(PathPoints[index].position);
        }
    }
}

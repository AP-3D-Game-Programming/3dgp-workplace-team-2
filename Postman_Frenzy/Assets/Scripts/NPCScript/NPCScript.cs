using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCScript : MonoBehaviour
{
    public NavMeshAgent agent;
    //public Animator animator;
    public GameObject PATH;
    public Transform[] PathPoints;

    public float minDistance = 5f;
    public int index = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //animator = GetComponent<Animator>();

        PathPoints = new Transform[PATH.transform.childCount];

        for (int i = 0; i < PathPoints.Length; i++)
        {
            PathPoints[i] = PATH.transform.GetChild(i);
        }
    }

    void Update()
    {
        Roam();
    }

    void Roam()
    {
        if (Vector3.Distance(transform.position, PathPoints[index].position) < minDistance)
        {
            index++;
            if (index == 3)
            {

                agent.SetDestination(PathPoints[index].position);
            }
            if (index >= PathPoints.Length)
            {
                index = 0;
            }
        }

        agent.SetDestination(PathPoints[index].position);
    }
}
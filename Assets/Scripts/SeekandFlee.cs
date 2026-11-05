
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SeekandFlee : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject target;
    public float distance = 5;
    public float fleeTime = 5;

    private bool fleeing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    void Flee(Vector3 location)
    {
        Vector3 fleeVector = transform.position - location;
        agent.SetDestination(transform.position + fleeVector.normalized * 10);
    }

    IEnumerator FleeCoroutine()
    {
        fleeing = true;

        float timer = 0;

        while (timer < fleeTime)
        {
            Flee(target.transform.position);

            timer += Time.deltaTime;

            yield return null;
        }

        fleeing = false;
    }

    void Update()
    {
        float vectordistance = Vector3.Distance(
            transform.position,
            target.transform.position
        );

        if (!fleeing && vectordistance <= distance)
        {
            StartCoroutine(FleeCoroutine());
        }

        if (!fleeing)
        {
            Seek(target.transform.position);
        }
    }
}

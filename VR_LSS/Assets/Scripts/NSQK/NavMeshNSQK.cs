using UnityEngine;
using UnityEngine.AI;

public class NavMeshNSQK : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;
    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player != null)
        {
            agent.destination = player.position;
        }
    }
}

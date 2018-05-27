using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
    [SerializeField] float maxHealthPoints = 100f;
    float currentHealthPoints = 100f;
    [SerializeField] float stopMoveDistance = 0.1f;
    [SerializeField] float aggroDistance = 6f;
    [SerializeField] float moveSpeed = 4;
    float distanceToPlayer;
    float distanceToOrigin;
    GameObject player;
    NavMeshAgent agent;

    Vector3 startPosition; //Will be set at position before moving. 
    

    public float healthAsPercentage {
        get {
            return currentHealthPoints / maxHealthPoints;
        }
    }

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        startPosition = transform.position;
        distanceToPlayer = this.DistanceToTarget(player.transform.position);
    }

    private void Update() {
        distanceToPlayer = this.DistanceToTarget(player.transform.position);
        if (distanceToPlayer < aggroDistance && distanceToPlayer > stopMoveDistance) {
            goTo(player.transform.position);
        } else {
            goTo(startPosition);
        }
    }

    float DistanceToTarget(Vector3 targetPosition){
        Vector3 displacement = targetPosition - transform.position;
        return displacement.magnitude;
    }

    void goTo(Vector3 targetPosition) {
        //Vector3 displacement = targetPosition - transform.position;
        //float distance = displacement.magnitude;
        //Vector3 direction = displacement.normalized;
        //if (distance > stopMoveDistance) {
        //    transform.LookAt(targetPosition);
        //    transform.position += direction * moveSpeed * Time.deltaTime;
        //}
        // agent.SetDestination(targetPosition);

        //Use navmesh agent for pathfinding.
        agent.SetDestination(targetPosition);
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowTarget : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    //private void OnCollisionEnter(Collision other)
    //{
    //    Debug.Log("Aleeeerta!");
    //    if (other.transform.tag == "PlayerBullet")
    //    {
    //        Debug.Log("Enemy Destroyed!");
    //        Destroy(gameObject);
    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "PlayerBullet")
        {
            Destroy(gameObject);
            SpawnManager.instance.DecreaseEnemyCount();
            Score.instance.IncreaseScore();
        }
    }

    private void LateUpdate()
    {
        agent.destination = target.position;
    }

    public void SlowDown(float duration)
    {

    }

    public NavMeshAgent GetNavMeshAgent() { return agent; }
}

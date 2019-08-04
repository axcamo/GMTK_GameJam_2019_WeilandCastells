using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGamePillar : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            SpawnManager.instance.StartSpawning();
            Destroy(this.gameObject);
        }
    }
}

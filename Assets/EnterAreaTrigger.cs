using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterAreaTrigger : MonoBehaviour
{
    [SerializeField] private Transform enemiesContainer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EnableEnemies();
        }
    }

    void EnableEnemies()
    {
        for (int i = 0; i < enemiesContainer.childCount; ++i)
        {
            enemiesContainer.GetChild(i).gameObject.SetActive(true);
        }
    }

    void DisableEnemies()
    {
        for (int i = 0; i < enemiesContainer.childCount; ++i)
        {
            enemiesContainer.GetChild(i).gameObject.SetActive(false);
        }
    }
}

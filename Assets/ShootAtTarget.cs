using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAtTarget : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float ratio;
    [SerializeField] private float shootAngleTolerance;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform bulletExit;
    [SerializeField] private Transform bulletParent;
    [SerializeField] private Transform target;

    private float timer;

    // Update is called once per frame
    void Update()
    {
        if (ratio <= 0) return;
        timer += Time.deltaTime;
        if(timer >= ratio && CheckIfLookingAtTarget())
        {
            Shoot();
            timer = 0;
        }
    }

    void Shoot()
    {
        GameObject b = Instantiate(bulletPrefab, bulletExit.position, transform.rotation, bulletParent);
        b.GetComponent<Bullet>().velocity = transform.forward * bulletSpeed;
    }

    bool CheckIfLookingAtTarget()
    {
        float a = Vector3.Angle(target.position - transform.position, transform.forward);

        if (a <= shootAngleTolerance) return true;
        else return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAtTarget : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float ratio;
    [SerializeField] private float shootAngleTolerance;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform weapon;
    [SerializeField] private Transform bulletParent;
    [SerializeField] private Animator animator;

    private FollowTarget followComponent;
    private AudioSource shotSound;
    private float timer;

    private void Start()
    {
        followComponent = GetComponent<FollowTarget>();
        shotSound = weapon.GetComponent<AudioSource>();
    }

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
        StartCoroutine("SlowDown", ratio*2);
        animator.SetTrigger("Shoot");
        GameObject b = Instantiate(bulletPrefab, weapon.position, transform.rotation, bulletParent);
        b.GetComponent<Bullet>().velocity = transform.forward * bulletSpeed;
        shotSound.volume = Random.Range(0.5f, 1.0f);
        shotSound.pitch = Random.Range(0.8f, 1.2f);
        shotSound.Play();
    }

    bool CheckIfLookingAtTarget()
    {
        Vector3 t = followComponent.target.position; t.y = 0;
        Vector3 me = transform.position;    me.y = 0;
        float a = Vector3.Angle(t - me, transform.forward);

        if (a <= shootAngleTolerance) return true;
        else return false;
    }

    IEnumerator SlowDown(float duration)
    {
        float defaultSpeed = followComponent.GetNavMeshAgent().speed;
        followComponent.GetNavMeshAgent().speed = 0;
        yield return new WaitForSeconds(duration);
        followComponent.GetNavMeshAgent().speed = defaultSpeed;
    }
}

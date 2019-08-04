using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 velocity;
    [SerializeField] private float lifetime;

    private void Awake()
    {
        Destroy(this.gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += velocity * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8) Destroy(this.gameObject);   // 8 = Level.
    }
}

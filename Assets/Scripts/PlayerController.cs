using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speedMagnitude;
    public float cameraSpeed;
    public float bulletTimeDuration;
    public float bulletSpeed;

    private bool onBulletTime;
    private float bulletTimeCounter;
    private bool canAttack;
    Vector2 moveVector;
    PlayerControls playerControls;
    CharacterController controller;
    Vector2 mouseDelta;

    [SerializeField] private GameObject weapon;
    [SerializeField] private GameObject bulletPrefab;

    public RectTransform bulletTimeBar;
    
    // Start is called before the first frame update
    void Awake()
    {

        controller = GetComponent<CharacterController>();
        playerControls = new PlayerControls();

        playerControls.Movement.Move.performed += ctx => { moveVector = ctx.ReadValue<Vector2>(); };
        playerControls.Movement.Move.canceled += ctx => moveVector = Vector2.zero;
        playerControls.Movement.Move.Enable();

        playerControls.Movement.RotateCamera.performed += ctx => { mouseDelta = ctx.ReadValue<Vector2>(); };
        playerControls.Movement.RotateCamera.canceled += ctx => { mouseDelta = Vector2.zero; };
        playerControls.Movement.RotateCamera.Enable();
    }

    private void Start()
    {
        canAttack = false;
        onBulletTime = false;
        bulletTimeCounter = 0;
    }

    void EditBulletTimeBar()
    {
        // min = 498
        // low2 + (value - low1) * (high2 - low2) / (high1 - low1)
        bulletTimeBar.localScale = new Vector3(1 + (bulletTimeCounter - bulletTimeDuration) * (0 - 1) / (0 - bulletTimeDuration), 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (onBulletTime)
        {
            if (bulletTimeCounter > 0)
            {
                Debug.Log("Counting: "+bulletTimeCounter);
                bulletTimeCounter -= Time.unscaledDeltaTime;
            }
            else
            {
                bulletTimeCounter = 0;
                DisableBulletTime();
            }
        }

        //Debug.Log(bulletTimeCounter);
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(canAttack)
                Shoot();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Damage();
        }
        //Camera
        Camera.main.transform.Rotate(0f, mouseDelta.x * cameraSpeed, 0f, Space.World);
        Camera.main.transform.Rotate(-mouseDelta.y * cameraSpeed, 0f, 0f, Space.Self);

        //Camera.main.transform.Rotate(new Vector3(mouseDelta.y, mouseDelta.x, 0));
        //transform.Translate(new Vector3(moveVector.x, 0, moveVector.y) * speedMagnitude);
        controller.SimpleMove((Camera.main.transform.forward * moveVector.y + Camera.main.transform.right * moveVector.x) * speedMagnitude);

        EditBulletTimeBar();
    }

    void Die()
    {

    }

    void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity))
        {
            GameObject b = Instantiate(bulletPrefab, weapon.transform.position, weapon.transform.rotation) as GameObject;
            b.tag = "PlayerBullet";
            b.transform.LookAt(hit.point);
            b.GetComponent<Bullet>().velocity = b.transform.forward * bulletSpeed;

            canAttack = false;
        }
    }

    void Damage()
    {
        if (canAttack)
            Die();
        else
        {
            Debug.Log("Damaged");

            EnableBulletTime();
        }
    }

    void EnableBulletTime()
    {
        bulletTimeCounter = bulletTimeDuration;
        canAttack = true;
        onBulletTime = true;
        Time.timeScale = 0.2f;
    }

    void DisableBulletTime()
    {
        if (canAttack)
            Die();

        canAttack = false;
        onBulletTime = false;
        Time.timeScale = 1f;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "EnemyBullet")
        {
            Damage();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "EnemyBullet")
        {
            Damage();
        }
    }

    void OnEnable()
    {
        playerControls.Movement.Move.Enable();
    }

    void OnDisable()
    {
        playerControls.Movement.Move.Disable();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public float speedMagnitude;
    public float cameraSpeed;
    public float bulletTimeDuration;
    public float bulletSpeed;

    private float targetFov;
    private bool onBulletTime;
    private float bulletTimeCounter;
    private bool canAttack;
    Vector2 moveVector;
    PlayerControls playerControls;
    CharacterController controller;
    Vector2 mouseDelta;

    [SerializeField] public CinemachineVirtualCamera cam;

    [SerializeField] private GameObject weapon;
    [SerializeField] private GameObject bulletPrefab;

    public RectTransform bulletTimeBar;
    
    // Start is called before the first frame update
    void Awake()
    {
        targetFov = 40;
        controller = GetComponent<CharacterController>();
        playerControls = new PlayerControls();

        playerControls.Movement.Move.performed += ctx => { moveVector = ctx.ReadValue<Vector2>(); };
        playerControls.Movement.Move.canceled += ctx => moveVector = Vector2.zero;
        playerControls.Movement.Move.Enable();

        //playerControls.Movement.RotateCamera.performed += ctx => { mouseDelta = ctx.ReadValue<Vector2>(); };
        //playerControls.Movement.RotateCamera.canceled += ctx => { mouseDelta = Vector2.zero; };
        //playerControls.Movement.RotateCamera.Enable();
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
        cam.m_Lens.FieldOfView = Mathf.Lerp(cam.m_Lens.FieldOfView, targetFov, Time.unscaledDeltaTime * 10);

        if (onBulletTime)
        {
            if (bulletTimeCounter > 0)
            {
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
        //float pitch = mouseDelta.x * cameraSpeed * Time.unscaledDeltaTime;
        //float yaw = -mouseDelta.y * cameraSpeed * Time.unscaledDeltaTime;

        //Debug.Log("Camera Motion: X:" + pitch + " Y:" + yaw);

        //Camera.main.transform.Rotate(0f, pitch, 0f, Space.World);
        //Camera.main.transform.Rotate(yaw, 0f, 0f, Space.Self);

        
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
        GameObject b = Instantiate(bulletPrefab, weapon.transform.position, weapon.transform.rotation) as GameObject;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity))
        {
            b.transform.LookAt(hit.point);
        }
        else
        {

        }

        b.GetComponent<Bullet>().velocity = b.transform.forward * bulletSpeed;
        canAttack = false;
    }

    void Damage()
    {
        if (canAttack)
            Die();
        else
        {
            EnableBulletTime();
        }
    }

    void EnableBulletTime()
    {
        targetFov = 60;
        bulletTimeCounter = bulletTimeDuration;
        canAttack = true;
        onBulletTime = true;
        Time.timeScale = 0.2f;
    }

    void DisableBulletTime()
    {
        if (canAttack)
            Die();

        targetFov = 40;
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

    void OnEnable()
    {
        playerControls.Movement.Move.Enable();
    }

    void OnDisable()
    {
        playerControls.Movement.Move.Disable();
    }
}

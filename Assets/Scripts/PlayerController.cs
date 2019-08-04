using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.Rendering.PostProcessing;

enum GameState { MENU, GAME, OVER }

public class PlayerController : MonoBehaviour
{
    public float speedMagnitude;
    public float cameraSpeed;
    public float bulletTimeDuration;
    public float bulletSpeed;

    private GameState state;
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

    [SerializeField] private GameObject gameCanvas;
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject gameOverCanvas;

    [SerializeField] private CinemachineVirtualCamera gameCamera;
    [SerializeField] private CinemachineVirtualCamera menuCamera;

    public RectTransform bulletTimeBar;

    public PostProcessProfile postProcessProfile;

    // Start is called before the first frame update
    void Awake()
    {
        state = GameState.MENU;
        targetFov = 40;
        controller = GetComponent<CharacterController>();
        playerControls = new PlayerControls();

        playerControls.Movement.Move.performed += ctx => { moveVector = ctx.ReadValue<Vector2>(); };
        playerControls.Movement.Move.canceled += ctx => moveVector = Vector2.zero;
        

        //playerControls.Movement.RotateCamera.performed += ctx => { mouseDelta = ctx.ReadValue<Vector2>(); };
        //playerControls.Movement.RotateCamera.canceled += ctx => { mouseDelta = Vector2.zero; };
        //playerControls.Movement.RotateCamera.Enable();
    }

    private void Start()
    {
        GameSystem.Instance.GameOverDelegate += Die;
        canAttack = false;
        onBulletTime = false;
        bulletTimeCounter = 0;
    }

    public void StartGame()
    {
        state = GameState.GAME;
        playerControls.Movement.Move.Enable();

        menuCamera.Priority = 0;
        gameCanvas.SetActive(true);
        menuCanvas.SetActive(false);
        SpawnManager.instance.StartSpawning();
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
        switch (state)
        {
            case GameState.MENU:
                {

                }
                break;
            case GameState.GAME:
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

                    if (Input.GetKeyDown(KeyCode.Mouse0))
                    {
                        if (canAttack)
                            Shoot();
                    }

                    controller.SimpleMove((Camera.main.transform.forward * moveVector.y + Camera.main.transform.right * moveVector.x) * speedMagnitude);

                    EditBulletTimeBar();
                }
                break;
            case GameState.OVER:
                {

                }
                break;
            default:
                break;
        }

        
    }

    void Die()
    {
        playerControls.Movement.Move.Disable();
        gameCanvas.SetActive(false);
        gameOverCanvas.SetActive(true);
        state = GameState.OVER;
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
            GameSystem.Instance.GameOverDelegate();
        else
        {
            EnableBulletTime();
        }
    }

    void EnableBulletTime()
    {
        postProcessProfile.GetSetting<ChromaticAberration>().active = true;
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

        postProcessProfile.GetSetting<ChromaticAberration>().active = false;
        targetFov = 40;
        canAttack = false;
        onBulletTime = false;
        Time.timeScale = 1f;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "EnemyBullet")
        {
            Destroy(other.transform.gameObject);
            Debug.Log("Ouch!");
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

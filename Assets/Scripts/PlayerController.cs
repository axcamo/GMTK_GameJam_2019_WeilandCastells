using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private bool canAttack;
    public float speedMagnitude;
    public float cameraSpeed;
    public float bulletTimeDuration;
    Vector2 moveVector;
    PlayerControls playerControls;
    CharacterController controller;
    Vector2 mouseDelta;

    
    
    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerControls = new PlayerControls();

        playerControls.Movement.Move.performed += ctx => { moveVector = ctx.ReadValue<Vector2>(); };
        playerControls.Movement.Move.canceled += ctx => moveVector = Vector2.zero;
        playerControls.Movement.Move.Enable();

        playerControls.Movement.RotateCamera.performed += ctx => { Debug.Log("MouseX"); mouseDelta = ctx.ReadValue<Vector2>(); };
        playerControls.Movement.RotateCamera.canceled += ctx => { Debug.Log("MouseY"); mouseDelta = Vector2.zero; };
        playerControls.Movement.RotateCamera.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        //Camera
        Camera.main.transform.Rotate(0f, mouseDelta.x * cameraSpeed, 0f, Space.World);
        Camera.main.transform.Rotate(-mouseDelta.y * cameraSpeed, 0f, 0f, Space.Self);

        //Camera.main.transform.Rotate(new Vector3(mouseDelta.y, mouseDelta.x, 0));
        //transform.Translate(new Vector3(moveVector.x, 0, moveVector.y) * speedMagnitude);
        controller.SimpleMove((Camera.main.transform.forward * moveVector.y + Camera.main.transform.right * moveVector.x) * speedMagnitude);
    }

    IEnumerator BulletTimeCoroutine()
    {
        EnableBulletTime();
        yield return new WaitForSeconds(bulletTimeDuration);
        canAttack = false;
        DisableBulletTime();
    }

    void Die()
    {

    }

    void Damage()
    {
        if (canAttack)
            Die();
        else
        {
            canAttack = true;
            StartCoroutine(BulletTimeCoroutine());
        }
    }

    void EnableBulletTime()
    {
        Time.timeScale = 0.2f;
    }

    void DisableBulletTime()
    {
        Time.timeScale = 1f;
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

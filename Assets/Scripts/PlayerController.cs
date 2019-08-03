using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speedMagnitude;
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

        playerControls.Camera.Rotate.performed += ctx => { mouseDelta = ctx.ReadValue<Vector2>(); };
        playerControls.Camera.Rotate.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.Rotate(mouseDelta, 10);
        //transform.Translate(new Vector3(moveVector.x, 0, moveVector.y) * speedMagnitude);
        controller.SimpleMove(new Vector3(moveVector.x, 0, moveVector.y) * speedMagnitude);
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

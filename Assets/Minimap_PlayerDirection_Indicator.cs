using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap_PlayerDirection_Indicator : MonoBehaviour
{
    private Camera camera;

    private void Start()
    {
        camera = Camera.main;
    }
    private void LateUpdate()
    {
        float angle = camera.transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }
}

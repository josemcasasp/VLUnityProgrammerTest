using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCamera : MonoBehaviour
{

    Camera fpsCamera;
    Transform player;
    public bool other = false;
    float hSpeed = 3;
    float vSpeed = 3;

    float xAxisClamp = 0;

    // Use this for initialization
    void Start()
    {
        fpsCamera = Camera.main;
        player = GameObject.FindWithTag("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {

        float h = hSpeed * Input.GetAxis("Mouse X");
        float v = vSpeed * Input.GetAxis("Mouse Y");

        xAxisClamp -= v;

        Vector3 rotPlayer = player.rotation.eulerAngles;
        Vector3 rotCamera = transform.rotation.eulerAngles;

        rotCamera.x -= v;
        rotCamera.z = 0;
        rotPlayer.y += h;

        if (xAxisClamp > 90)
        {
            xAxisClamp = 90;
            rotCamera.x = 90;
        }
        else if (xAxisClamp < -90)
        {
            xAxisClamp = -90;
            rotCamera.x = 270;
        }

        transform.rotation = Quaternion.Euler(rotCamera);
        player.rotation = Quaternion.Euler(rotPlayer);


    }



}

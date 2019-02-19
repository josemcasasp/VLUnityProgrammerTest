using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCamera : MonoBehaviour {

    Camera tpsCamera;
    Transform player;
    public Vector3 fpsCameraPos;
    public Transform tpsCameraPos;
    bool moving = false;
    public bool other = false;
    float hSpeed = 2;
    float vSpeed = 2;

    float AxisClamp = 0;

    // Use this for initialization
    void Start () {

        tpsCamera = Camera.main;
        player = GameObject.FindWithTag("Player").transform;

    }

    // Update is called once per frame
    void Update () {

        float h = hSpeed * Input.GetAxis("Mouse X");
        float v = vSpeed * Input.GetAxis("Mouse Y");

        AxisClamp -= v;

        Vector3 rotPlayer = player.rotation.eulerAngles;
        Vector3 rotCamera = transform.rotation.eulerAngles;

        rotCamera.x -= v;
        rotCamera.z = 0;
        rotPlayer.y += h;


        if (AxisClamp > 50)
        {
            AxisClamp = 50;
            rotCamera.x = 50;

        }
        else if (AxisClamp < -45)
        {
            AxisClamp = -45;
            rotCamera.x = 315;

        }

        transform.rotation = Quaternion.Euler(rotCamera);
        player.rotation = Quaternion.Euler(rotPlayer);

    }
}

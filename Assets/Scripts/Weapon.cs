using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{


    public enum Weapons { Machinegun, Sniper, Shotgun }
    public Weapons weapon;

    private Camera fpsCamera;

    public float firingRate;
    public int damage;
    public float dispersion;
    public float range = 50;

    float nextFire = 1;

    // Use this for initialization
    void Start()
    {

        fpsCamera = Camera.main;

        //Set weapon
        switch (weapon)
        {
            case Weapons.Machinegun:
                firingRate = 20;
                damage = 3;
                break;
            case Weapons.Sniper:
                firingRate = 0.5f;
                damage = 15;
                break;
            case Weapons.Shotgun:
                firingRate = 1;
                damage = 5;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetButton("Fire1") && nextFire <= 0)
        {
            if (weapon == Weapons.Shotgun)
            {
                ShotGun();
            }
            else
            {
                Shoot();
            }
            nextFire = 1;
        }
        nextFire -= Time.deltaTime * firingRate;

    }

    void Shoot()
    {
        RaycastHit hit;

        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            if (hit.transform.tag == "Enemy") { hit.transform.GetComponent<EnemyBehaviour>().OnDamage(damage); }
        }
    }

    void ShotGun()
    {
        int pellets = 15;

        for (var i = 0; i < pellets; i++)
        {

            RaycastHit hit;
            //var dispersion = transform.up * Random.Range(0.0f, 2);
            var dispersion = transform.up * Random.Range(0.0f, 2);
            dispersion = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), transform.forward) * dispersion;
            var v3Hit = fpsCamera.transform.forward * range + dispersion;

            if (Physics.Raycast(fpsCamera.transform.position, v3Hit, out hit, range))
            {
                Debug.Log(hit.transform.name);

                if (hit.transform.tag == "Enemy")
                {
                    hit.transform.GetComponent<EnemyBehaviour>().OnDamage(damage);
                }
            }

        }


    }
}

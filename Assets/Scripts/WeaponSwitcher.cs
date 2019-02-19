using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour {

    public int activeWeapon = 0;

	// Use this for initialization
	void Start () {
        SelectWeapon();
	}
	
	// Update is called once per frame
	void Update () {

        int previousActiveWeapon = activeWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (activeWeapon >= transform.childCount - 1)
                activeWeapon = 0;
            else
                activeWeapon++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (activeWeapon <= 0)
                activeWeapon = transform.childCount -1;
            else
                activeWeapon--;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) activeWeapon = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2) activeWeapon = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3) activeWeapon = 2;

        if (previousActiveWeapon != activeWeapon) SelectWeapon();


    }

    void SelectWeapon()
    {
        int index = 0;
        foreach (Transform weapon in transform)
        {
            if (index == activeWeapon) weapon.gameObject.SetActive(true);
            else weapon.gameObject.SetActive(false);
            index++;
        }
    }
}

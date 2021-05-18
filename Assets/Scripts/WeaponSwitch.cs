using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public GameObject[] Weapons;
    public int currentWeapon;

    void Start()
    {
        Switch(Weapons.Length - 1);   
    }

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        { 
            Switch(currentWeapon);
        }
    }

    void Switch(int weaponType)
    {
        currentWeapon = (weaponType + 1) % Weapons.Length;
        for (int i = 0; i < Weapons.Length; i++) 
        {
            if (i == currentWeapon)
            {
                Weapons[i].SetActive(true);
            }
            else
            {
                Weapons[i].SetActive(false);
            }
        }
    }
}

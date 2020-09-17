using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmeManager : MonoBehaviour
{
    public GameObject[] weapons;
    public int Current_weapon;
    public int New_weapon;
    public float Delay;
    public bool IsSwitching = false;

    // Start is called before the first frame update
    void Start()
    {
        checkWeapon();
       
    }

    private void checkWeapon()
    {
        for (int i=0; i<weapons.Length;i++)
        {
            if(i!=Current_weapon)
            {
                weapons[i].SetActive(false);
            }
            else
            {
                weapons[i].SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (Current_weapon != 0)
            {
                Current_weapon = 0;
            }
            StartCoroutine("waitTime");
            checkWeapon();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
                if (Current_weapon != 1)
                {
                    Current_weapon = 1;
                }

            StartCoroutine("waitTime");
            checkWeapon();
        }
    }

    IEnumerator waitTime()
    {
        IsSwitching = true;
        yield return new WaitForSeconds(Delay);
        IsSwitching = false;
    }
}

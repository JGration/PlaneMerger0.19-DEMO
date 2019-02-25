using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowWeapon : MonoBehaviour {

    public GameObject LongswordHUD;
    public GameObject DualswordHUD;
    public GameObject GreatswordHUD;
    public int weaponSelected;


    void Update () {
        weaponSelected = GameObject.FindGameObjectWithTag("WeaponHolder").GetComponent<WeaponSwitching>().selectedWeapon;
        if (weaponSelected == 0)
        {
            LongswordHUD.SetActive(true);
            DualswordHUD.SetActive(false);
            GreatswordHUD.SetActive(false);
        }     
        else if (weaponSelected == 1)
        {
            LongswordHUD.SetActive(false);
            DualswordHUD.SetActive(true);
            GreatswordHUD.SetActive(false);
        }
        else if (weaponSelected == 2)
        {
            LongswordHUD.SetActive(false);
            DualswordHUD.SetActive(false);
            GreatswordHUD.SetActive(true);
        }
    }
}

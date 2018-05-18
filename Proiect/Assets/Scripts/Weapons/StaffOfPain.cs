﻿using UnityEngine;

public class StaffOfPain : MonoBehaviour
{
    private static Weapon staffOfPain;
    private CharacterController playerController;
    // the time of the last attack
    private float timeOfLastAttack;
    // the time since the last secondary attack
    private float timeOfSecAttack;

    void Awake()
    {
        playerController = GetComponentInParent<CharacterController>();

        timeOfLastAttack = 0;
        timeOfSecAttack = 0;

        staffOfPain = new Weapon
        {
            WeaponName = "Staff Of Pain",
            RateOfFire = 1,
            SecondRateOfFire = 1.5f,
            MaxAmmo = -1,
            InitialAmmo = -1,
            Damage = 20,
            SecondaryDamage = 10,
            Range = 0,
            AmmoType = null,
            WeaponPosition = new Vector3(0.2f, -0.5f, 0.5f),
            WeaponRotation = Quaternion.Euler(5, 0, -15),
            WeaponAnimator = GetComponent<Animator>(),
            AllowAnimations = true
        };
    }

    public void FixedUpdate()
    {
        if (staffOfPain.AllowAnimations)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                if (playerController.isGrounded)
                    staffOfPain.WeaponAnimator.SetBool("Run", true);
            }
            else
                staffOfPain.WeaponAnimator.SetBool("Run", false);
        }

        if (Input.GetButtonDown("Fire1") && Time.time - timeOfLastAttack >= staffOfPain.RateOfFire)
        {
            timeOfLastAttack = Time.time;
            staffOfPain.WeaponAnimator.SetTrigger("PrimaryFire");
        }

        if (Input.GetButtonDown("Fire2") && Time.time - timeOfSecAttack >= staffOfPain.SecondRateOfFire)
        {
            timeOfSecAttack = Time.time;
            staffOfPain.WeaponAnimator.SetTrigger("SecondaryFire");
        }
    }

    public static Weapon AttachedWeapon
    {
        get
        {
            return staffOfPain;
        }
    }

    // primary attack of the weapon
    public void PrimaryFire()
    {
        
    }

    // secondary attack of the weapon
    public void SecondaryFire()
    {
        
    }
}

using UnityEngine;

public class BloodyAxe : MonoBehaviour
{
    private static Weapon bloodyAxe;
    private CharacterController playerController;
    private int consecutiveAttacks;
    // the time of the last attack
    private float timeOfLastAttack;

    void Awake ()
    {
        playerController = GetComponentInParent<CharacterController>();

        timeOfLastAttack = 0;
        consecutiveAttacks = 0;

        bloodyAxe = new Weapon
        {
            WeaponName = "Bloody Axe",
            RateOfFire = 1,
            SecondRateOfFire = 0,
            MaxAmmo = -1,
            InitialAmmo = -1,
            Damage = 20,
            SecondaryDamage = 0,
            Range = 0,
            AmmoName = null,
            WeaponPosition = new Vector3(0.2f, -0.45f, 0.45f),
            WeaponRotation = Quaternion.Euler(-65, 50, -50),
            WeaponAnimator = GetComponent<Animator>(),
            AllowAnimations = true
        };
    }

    public void FixedUpdate()
    {
        if (bloodyAxe.AllowAnimations)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                if (playerController.isGrounded)
                    bloodyAxe.WeaponAnimator.SetBool("Run", true);
            }
            else
                bloodyAxe.WeaponAnimator.SetBool("Run", false);
        }

        if (Input.GetButtonDown("Fire1") && Time.time - timeOfLastAttack >= bloodyAxe.RateOfFire)
            PrimaryFire();
    }

    public static Weapon AttachedWeapon
    {
        get
        {
            return bloodyAxe;
        }
    }

    // primary attack of the weapon
    public void PrimaryFire()
    {
        timeOfLastAttack = Time.time;

        if (Random.Range(1, 11) <= consecutiveAttacks)
        {
            bloodyAxe.WeaponAnimator.SetTrigger("ContinueAttack");
            consecutiveAttacks = 0;
        }
        else
        {
            bloodyAxe.WeaponAnimator.SetTrigger("Attack");
            consecutiveAttacks+=2;
        }
    }

    // secondary attack of the weapon
    public void SecondaryFire()
    {

    }
}

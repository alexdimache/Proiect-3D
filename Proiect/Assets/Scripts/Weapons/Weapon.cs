using UnityEngine;

public class Weapon
{
    // the name of the weapon
    private string weaponName;

    // how fast the primary attack shoots
    private float rateOfFire;

    // how fast the secondary attack shoots
    private float secondRateOfFire;

    // the maximum amount of ammo
    private int maxAmmo;

    // the initial amount of ammo (on pickup)
    private int initialAmmo;

    // the damage it deals
    private int damage;

    // the damage dealt by the secondary attack
    private int secondaryDamage;

    // the range of the weapon
    private float range;

    // the type of ammo
    private AmmoType ammoType;

    // position of the weapon
    private Vector3 weaponPosition;

    // rotation of the weapon
    private Quaternion weaponRotation;

    // the animator attached to the weapon
    private Animator weaponAnimator;

    // flag to allow animations or not
    private bool allowAnimations;

    // Get the attributes
    public string WeaponName
    {
        get { return weaponName; }
        set { weaponName = value; }
    }

    public float RateOfFire
    {
        get { return rateOfFire; }
        set { rateOfFire = value; }
    }

    public float SecondRateOfFire
    {
        get { return secondRateOfFire; }
        set { secondRateOfFire = value; }
    }

    public int MaxAmmo
    {
        get { return maxAmmo; }
        set { maxAmmo = value; }
    }

    public int InitialAmmo
    {
        get { return initialAmmo; }
        set { initialAmmo = value; }
    }

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    public int SecondaryDamage
    {
        get { return secondaryDamage; }
        set { secondaryDamage = value; }
    }

    public float Range
    {
        get { return range; }
        set { range = value; }
    }

    public AmmoType AmmoType
    {
        get { return ammoType; }
        set { ammoType = value; }
    }

    public Vector3 WeaponPosition
    {
        get { return weaponPosition; }
        set { weaponPosition = value; }
    }

    public Quaternion WeaponRotation
    {
        get { return weaponRotation; }
        set { weaponRotation = value; }
    }

    public Animator WeaponAnimator
    {
        get { return weaponAnimator; }
        set { weaponAnimator = value; }
    }

    public bool AllowAnimations
    {
        get { return allowAnimations; }
        set { allowAnimations = value; }
    }
}

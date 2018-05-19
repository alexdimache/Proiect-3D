using UnityEngine;

public class AmmoType: MonoBehaviour
{
    // the projectile's name
    private string ammoName;

    // how much ammo is in one pack
    private int quantity;

    public AmmoType(string givenName, int givenQuantity)
    {
        ammoName = givenName;
        quantity = givenQuantity;
    }

    public string AmmoName
    {
        get { return ammoName; }
    }

    public int Quantity
    {
        get { return quantity; }
    }
}

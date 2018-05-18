using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // when the HP reaches 0 the player dies
    private int hitPoints;
    // the armor points
    private int armorPoints;
    // list of effects
    private List<ActiveEffect> activeEffects;
    // weapon holder instance
    private WeaponHolder weaponHolder;

	// Use this for initialization
	void Start ()
    {
        hitPoints = 100;
        armorPoints = 0;

        activeEffects = new List<ActiveEffect>();
        weaponHolder = GetComponentInChildren<WeaponHolder>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        
    }

    public int HP
    {
        get { return hitPoints; }
        set { hitPoints = value; }
    }

    public int Armor
    {
        get { return armorPoints; }
        set { armorPoints = value; }
    }

    public void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "WeaponPickup":
                    weaponHolder.AddWeapon(other.gameObject.GetComponent<WeaponPickup>().GetPrefab);
                    break;
            default:
                Debug.Log("OTHER TRIGGER");
                break;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    // the list with the weapons
    [SerializeField]
    private List<GameObject> weaponList;
    // the weapon currently selected
    private GameObject selectedWeapon;

    // Use this for initialization
    void Start()
    {
        weaponList = new List<GameObject>(5);
        selectedWeapon = null;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Bloody Axe"))
            SetActiveWeapon("Bloody Axe");
        if (Input.GetButtonDown("King Axe"))
            SetActiveWeapon("King Axe");
        if (Input.GetButtonDown("Dragonblade"))
            SetActiveWeapon("Dragonblade");
        if (Input.GetButtonDown("Staff Of Pain"))
            SetActiveWeapon("Staff Of Pain");
        if (Input.GetButtonDown("Emerald Staff"))
            SetActiveWeapon("Emerald Staff");
    }

    public void AddWeapon(GameObject givenWeapon)
    {
        GameObject tempInstance = Instantiate(givenWeapon, transform);
        givenWeapon.SetActive(false);
        switch (givenWeapon.name)
        {
            case "Bloody Axe":
                tempInstance.transform.localPosition = BloodyAxe.AttachedWeapon.WeaponPosition;
                tempInstance.transform.localRotation = BloodyAxe.AttachedWeapon.WeaponRotation;

                weaponList.Add(givenWeapon);
                SetActiveWeapon(givenWeapon.name);
                break;

            case "Staff Of Pain":
                Debug.Log(StaffOfPain.AttachedWeapon.WeaponPosition);
                tempInstance.transform.localPosition = StaffOfPain.AttachedWeapon.WeaponPosition;
                tempInstance.transform.localRotation = StaffOfPain.AttachedWeapon.WeaponRotation;
                
                weaponList.Add(givenWeapon);
                break;
        }
        
    }

    public void AddAmmo(string ammoName)
    {

    }

    private GameObject FindWeapon(string givenName)
    {
        foreach (GameObject obj in weaponList)
            if (obj.name.Equals(givenName))
                return obj;
        return null;
    }

    public void SetActiveWeapon(string weaponName)
    {
        if (FindWeapon(weaponName) != null)
        {
            if (selectedWeapon != null)
                selectedWeapon.SetActive(false);

            selectedWeapon = FindWeapon(weaponName);
            selectedWeapon.SetActive(true);
            Debug.Log("SELECTED " + selectedWeapon.ToString());
        } 
    }
}

using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    // the list with the weapons
    private List<GameObject> weaponList;
    // the weapon currently selected
    private GameObject selectedWeapon;
    // game object for the projectiles
    private GameObject projectilePool;
    // object pool for the weapon projectiles
    private Dictionary<string, Queue<GameObject>> objectPool;

    // Use this for initialization
    void Start()
    {
        weaponList = new List<GameObject>(5);
        projectilePool = transform.Find("_ProjectilePool").gameObject;
        objectPool = new Dictionary<string, Queue<GameObject>>();
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

                // instantiating 12 projectiles of every type
                Queue<GameObject> tempPrimaryPool = new Queue<GameObject>();
                Queue<GameObject> tempSecondaryPool = new Queue<GameObject>();
                for(int i = 0; i < 12; i++)
                {
                    GameObject obj = Instantiate(givenWeapon.GetComponent<StaffOfPain>().PrimaryProjectile, projectilePool.transform);
                    obj.SetActive(false);
                    tempPrimaryPool.Enqueue(obj);
                    obj = Instantiate(givenWeapon.GetComponent<StaffOfPain>().SecondaryProjectile, projectilePool.transform);
                    obj.SetActive(false);
                    tempSecondaryPool.Enqueue(obj);
                }
                objectPool.Add("Primary_" + givenWeapon.name, tempPrimaryPool);
                objectPool.Add("Secondary_" + givenWeapon.name, tempSecondaryPool);

                break;
        }
        //givenWeapon.SetActive(false);

    }

    public GameObject GetProjectile(string projectileName)
    {
        return objectPool[projectileName].Dequeue();
    }

    public void AddProjectile(string projectileName, GameObject givenProjectile)
    {
        objectPool[projectileName].Enqueue(givenProjectile);
        givenProjectile.SetActive(false);
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

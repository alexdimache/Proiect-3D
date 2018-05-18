using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    private string pickupName;
    [SerializeField]
    private GameObject weaponPrefab;

    private void Start()
    {
        pickupName = transform.parent.gameObject.name;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            DestroyObject(transform.parent.gameObject);
    }

    public string Name
    {
        get { return pickupName; }
    }

    public GameObject GetPrefab
    {
        get { return weaponPrefab; }
    }
}

using UnityEngine;

public class SoP_Primary : MonoBehaviour
{
    private WeaponHolder holderInstance;
    private Rigidbody projectileBody;
    private int damage;
    private string projectileName;
    private float projectileRange;
    private Vector3 projectileTransform;

	// Use this for initialization
	void Start ()
    {
        projectileBody = GetComponent<Rigidbody>();
        holderInstance = transform.parent.transform.parent.GetComponent<WeaponHolder>();
        damage = StaffOfPain.AttachedWeapon.Damage;
        projectileName = "Primary_" + StaffOfPain.AttachedWeapon.WeaponName;
        projectileRange = StaffOfPain.AttachedWeapon.Range;
        projectileTransform = transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Vector3.Distance(projectileTransform, transform.position) >= projectileRange)
            holderInstance.AddProjectile(projectileName, transform.parent.gameObject);

        projectileBody.AddForce(Vector3.forward/100, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
            holderInstance.AddProjectile(projectileName, gameObject);
    }
}

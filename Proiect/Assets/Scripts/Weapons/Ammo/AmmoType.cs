public abstract class AmmoType
{
    // the projectile's name
    private string name;

    // the amount of force it has
    private float force;

    // how much ammo is in one pack
    private int quantity;

    public string Name
    {
        get { return name; }
        protected set { name = value; }
    }

    public float Force
    {
        get { return force; }
        protected set { force = value; }
    }

    public int Quantity
    {
        get { return quantity; }
        protected set { quantity = value; }
    }
}

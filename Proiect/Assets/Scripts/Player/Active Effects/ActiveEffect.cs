public abstract class ActiveEffect
{
    // the effects that remove this one
    private string[] nullingEffects;

    // the name of the effect
    private string effectName;

    // the damage it deals initially
    private int upfrontDamage;

    // the duration of the effect
    private float effectDuration;

    // the damage it deals per second
    private float effectDamage;

    // the amount it has on the damage the player deals
    private float damageDealtMultiplier;

    // how much more damage you receive while the effect is active
    private float damageReceivedMultiplier;

    // the impact it has on the player's speed
    private float effectSpeed;

    // Get the attributes
    public string[] NullingEffects
    {
        get { return nullingEffects; }
        protected set { nullingEffects = value; }
    }

    public string EffectName
    {
        get { return effectName; }
        protected set { effectName = value; }
    }

    public int UpfrontDamage
    {
        get { return upfrontDamage; }
        protected set { upfrontDamage = value; }
    }

    public float EffectDuration
    {
        get { return effectDuration; }
        protected set { effectDuration = value; }
    }

    public float EffectDamage
    {
        get { return effectDamage; }
        protected set { effectDamage = value; }
    }

    public float DamageDealtMul
    {
        get { return damageDealtMultiplier; }
        protected set { damageDealtMultiplier = value; }
    }

    public float DamageReceivedMul
    {
        get { return damageReceivedMultiplier; }
        protected set { damageReceivedMultiplier = value; }
    }

    public float EffectSpeed
    {
        get { return effectSpeed; }
        protected set { effectSpeed = value; }
    }

    // these methods do nothing by default
    public virtual void OnHitReceived() { }
    public virtual void OnHitDealt() { }
}

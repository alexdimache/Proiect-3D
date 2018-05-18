public class Cursed : ActiveEffect {

    public Cursed()
    {
        NullingEffects = null;
        EffectName = "Cursed";
        UpfrontDamage = 6;
        EffectDuration = 6;
        EffectDamage = 0.3f;
        DamageDealtMul = 0.6f;
        DamageReceivedMul = 1.6f;
        EffectSpeed = 0.666f;
    }

    public override void OnHitReceived()
    {
        EffectDuration += 1;
    }

    public void OnHitDealt(int damageDealt)
    {
        EffectDamage = 0.3f * damageDealt;
    }

}

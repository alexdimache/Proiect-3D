public class Freezing : ActiveEffect
{
    public Freezing()
    {
        NullingEffects = new string[] { "Burning" };
        EffectName = "Freezing";
        UpfrontDamage = 10;
        EffectDuration = 6;
        EffectDamage = 1;
        DamageDealtMul = 0.85f;
        DamageReceivedMul = 1.15f;
        EffectSpeed = 0.3f;
    }

    // with every hit received the player gets slowed even more up to a maximum of 0.9
    public override void OnHitReceived()
    {
        if (EffectSpeed < 0.9)
            EffectSpeed += 0.3f;
    }

    // nothing happens when the player deals damage while this effect is active
    public void OnHitDealt(Player givenPlayer)
    {
        // nothing happens
    }
}

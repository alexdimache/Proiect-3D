public class Burning : ActiveEffect
{
    public Burning()
    {
        NullingEffects = new string[] { "Freezing" };
        EffectName = "Burning";
        UpfrontDamage = 5;
        EffectDuration = 5;
        EffectDamage = 5;
        DamageDealtMul = 1.3f;
        DamageReceivedMul = 1;
        EffectSpeed = 1;
    }

    // with every hit received the effect's duration resets
    public override void OnHitReceived()
    {
        if (DamageReceivedMul < 1.3)
            DamageReceivedMul += 0.1f;
    }

    // nothing happens when the player deals damage while this effect is active
    public void OnHitDealt(Player givenPlayer)
    {
        // nothing happens
    }
}

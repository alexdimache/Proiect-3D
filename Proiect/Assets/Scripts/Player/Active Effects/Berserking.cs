public class Berserking : ActiveEffect
{
    public Berserking()
    {
        NullingEffects = null;
        EffectName = "Berserking";
        UpfrontDamage = 0;
        EffectDuration = 10;
        EffectDamage = 0;
        DamageDealtMul = 2f;
        DamageReceivedMul = 1.5f;
        EffectSpeed = 1;
    }

    // with every hit received the effect loses a second off of its duration
    public override void OnHitReceived()
    {
        EffectDuration -= 1;
    }

    // with every hit dealt the player gets 1% of his HP back
    public void OnHitDealt(Player givenPlayer)
    {
        givenPlayer.HP += givenPlayer.HP / 100;
    }
}

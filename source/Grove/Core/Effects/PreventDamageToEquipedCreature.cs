namespace Grove.Effects
{
  using System;
  using Modifiers;

  public class PreventDamageToEquipedCreature : Effect
  {
    private readonly Func<Card, int> _amount;

    private PreventDamageToEquipedCreature() {}

    public PreventDamageToEquipedCreature(Func<Card, int> amount)
    {
      _amount = amount;
    }

    protected override void ResolveEffect()
    {
      var mp = new ModifierParameters
        {
          SourceCard = Source.OwningCard,
          SourceEffect = this,
        };

      var prevention = new PreventDamageToTarget(
        target: Source.OwningCard.AttachedTo,
        amount: (creatureOrPlayer, ctx) => _amount((Card) (creatureOrPlayer)));

      var modifier = new AddDamagePrevention(prevention);
      Game.AddModifier(modifier, mp);
    }
  }
}
namespace Grove.Gameplay.Effects
{
  using DamageHandling;
  using Modifiers;

  public class PreventCombatDamage : Effect
  {
    protected override void ResolveEffect()
    {
      var mp = new ModifierParameters
        {
          SourceCard = Source.OwningCard,
          SourceEffect = this,        
        };

      var prevention = new PreventAllCombatDamage();
      var modifier = new AddDamagePrevention(prevention) {UntilEot = true};      
      Game.AddModifier(modifier, mp);
    }
  }
}
namespace Grove.Effects
{
  using Modifiers;

  public class PreventAllCombatDamage : Effect
  {
    protected override void ResolveEffect()
    {
      var mp = new ModifierParameters
        {
          SourceCard = Source.OwningCard,
          SourceEffect = this,        
        };

      var prevention = new Grove.PreventAllCombatDamage();
      var modifier = new AddDamagePrevention(prevention) {UntilEot = true};      
      Game.AddModifier(modifier, mp);
    }
  }
}
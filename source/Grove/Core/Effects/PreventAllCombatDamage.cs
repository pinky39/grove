namespace Grove.Effects
{
  using System;
  using Modifiers;

  public class PreventAllCombatDamage : Effect
  {
    private readonly Func<Card, bool> _filter;
    private PreventAllCombatDamage() { }

    public PreventAllCombatDamage(Func<Card, bool> filter = null)
    {
      _filter = filter ?? delegate { return true; };
    }

    protected override void ResolveEffect()
    {
      var mp = new ModifierParameters
        {
          SourceCard = Source.OwningCard,
          SourceEffect = this,        
        };

      var prevention = new Grove.PreventCombatDamage(_filter);
      var modifier = new AddDamagePrevention(prevention) {UntilEot = true};      
      Game.AddModifier(modifier, mp);
    }
  }
}
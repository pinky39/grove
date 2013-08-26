namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Gameplay.Costs;  
  using Gameplay.Misc;

  public class SanctumCustodian : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Sanctum Custodian")
        .ManaCost("{2}{W}")
        .Type("Creature Human Cleric")
        .Text("{T}: Prevent the next 2 damage that would be dealt to target creature or player this turn.")
        .FlavorText("Serra told them to guard Urza as he healed. Five years they stood.")
        .Power(1)
        .Toughness(2)
        .ActivatedAbility(p =>
          {
            p.Text = "{T}: Prevent the next 2 damage that would be dealt to target creature or player this turn.";
            p.Cost = new Tap();
            p.Effect = () => new Gameplay.Effects.PreventNextDamageToTargets(2);
            p.TargetSelector.AddEffect(trg => trg.Is.CreatureOrPlayer().On.Battlefield());
            p.TargetingRule(new EffectPreventNextDamageToTargets(2));
          }
        );
    }
  }
}
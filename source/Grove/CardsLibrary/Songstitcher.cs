namespace Grove.CardsLibrary
{
  using System.Collections.Generic;
  using AI.TargetingRules;
  using AI.TimingRules;
  using Costs;
  using Effects;

  public class Songstitcher : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Songstitcher")
        .ManaCost("{W}")
        .Type("Creature Human Cleric")
        .Text(
          "{1}{W}: Prevent all combat damage that would be dealt this turn by target attacking creature with flying.")
        .FlavorText("The true names of birds are songs woven into their souls.")
        .Power(1)
        .Toughness(2)
        .ActivatedAbility(p =>
          {
            p.Text =
              "{1}{W}: Prevent all combat damage that would be dealt this turn by target attacking creature with flying.";

            p.Cost = new PayMana("{1}{W}".Parse());
            p.Effect = () => new PreventAllDamageFromSource(preventCombatOnly: true);
            p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Has().Flying && c.IsAttacker).On.Battlefield());
            p.TargetingRule(new EffectPreventCombatDamage());
            p.TimingRule(new AfterOpponentDeclaresAttackers());
          });
    }
  }
}
namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TargetingRules;
  using Core.Ai.TimingRules;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Modifiers;
  using Core.Preventions;

  public class Songstitcher : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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

            p.Cost = new PayMana("{1}{W}".ParseMana(), ManaUsage.Abilities);
            p.Effect =
              () =>
                new ApplyModifiersToTargets(() => new AddDamagePrevention(new PreventCombatDamage()) {UntilEot = true});
            p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Has().Flying && c.IsAttacker).On.Battlefield());
            p.TargetingRule(new PreventDamageFromAttackers());
            p.TimingRule(new Turn(passive: true));
            p.TimingRule(new Steps(Step.DeclareAttackers));
          });
    }
  }
}
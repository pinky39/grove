namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay.AI.TargetingRules;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay;
  using Grove.Gameplay.Costs;
  using Grove.Gameplay.Effects;

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

            p.Cost = new PayMana("{1}{W}".Parse(), ManaUsage.Abilities);
            p.Effect = () => new PreventDamageFromSource();
            p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Has().Flying && c.IsAttacker).On.Battlefield());
            p.TargetingRule(new EffectPreventAttackerDamage());            
            p.TimingRule(new OnOpponentsTurn(Step.DeclareAttackers));
          });
    }
  }
}
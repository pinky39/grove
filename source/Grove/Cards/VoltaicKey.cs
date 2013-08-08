namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.States;

  public class VoltaicKey : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Voltaic Key")
        .ManaCost("{1}")
        .Type("Artifact")
        .Text("{1},{T}: Untap target artifact.")
        .FlavorText("The key did not work on a single lock, yet it opened many doors.")
        .ActivatedAbility(p =>
          {
            p.Text = "{1},{T}: Untap target artifact.";

            p.Cost = new AggregateCost(
              new PayMana(1.Colorless(), ManaUsage.Abilities),
              new Tap());

            p.Effect = () => new UntapTargetPermanents();
            p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Artifact).On.Battlefield());

            p.TimingRule(new Steps(Step.FirstMain, Step.SecondMain));
            p.TargetingRule(new OrderByRank(c => -c.Score, ControlledBy.SpellOwner) {ConsiderTargetingSelf = false});
          });
    }
  }
}
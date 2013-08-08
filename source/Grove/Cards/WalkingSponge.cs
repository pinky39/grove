namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TargetingRules;
  using Artifical.TimingRules;
  using Gameplay.Abilities;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.States;

  public class WalkingSponge : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Walking Sponge")
        .Type("Creature Sponge")
        .ManaCost("{1}{U}")
        .Text("{T}: Target creature loses your choice of flying, first strike, or trample until end of turn.")
        .FlavorText("Not only does it catch fish, it cleans them too.")
        .Power(1)
        .Toughness(1)
        .ActivatedAbility(p =>
          {
            p.Text = "{T}: Target creature loses your choice of flying, first strike, or trample until end of turn.";
            p.Cost = new Tap();
            p.Effect = () => new TargetLoosesChosenAbility(Static.Flying, Static.FirstStrike, Static.Trample);
            p.TargetSelector.AddEffect(trg => trg.Is.Creature().On.Battlefield());

            p.TargetingRule(new LooseEvasion(Static.Flying, Static.FirstStrike, Static.Trample));
            p.TimingRule(new Steps(activeTurn: true, passiveTurn: true, steps: Step.BeginningOfCombat));
          });
    }
  }
}
namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay.Abilities;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;
  using Gameplay.States;

  public class HeadlongRush : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Headlong Rush")
        .ManaCost("{1}{R}")
        .Type("Instant")
        .Text("Attacking creatures gain first strike until end of turn.")
        .FlavorText(
          "A landslide of goblins poured towards the defenders—tumbling, rolling, and bouncing their way down the steep hillside.")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToPermanents(
              permanentFilter: (e, c) => c.IsAttacker,
              modifiers: () => new AddStaticAbility(Static.FirstStrike) {UntilEot = true});

            p.TimingRule(new Turn(active: true));
            p.TimingRule(new Steps(Step.DeclareBlockers));
            p.TimingRule(new MinAttackerCount(1));
          });
    }
  }
}
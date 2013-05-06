namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.TimingRules;
  using Gameplay;
  using Gameplay.Abilities;
  using Gameplay.Costs;
  using Gameplay.Effects;
  using Gameplay.ManaHandling;
  using Gameplay.Misc;
  using Gameplay.Modifiers;
  using Gameplay.States;

  public class RainOfFilth : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Rain of Filth")
        .ManaCost("{B}")
        .Type("Instant")
        .Text("Until end of turn, lands you control gain 'Sacrifice this land: Add {B} to your mana pool.'")
        .FlavorText("When I say it rained, it was not small drops, but a thick, greasy drool pouring from the heavens.")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToPermanents(
              permanentFilter: (e, c) => c.Is().Land,
              controlledBy: ControlledBy.SpellOwner,
              modifiers: () =>
                {
                  var mp = new ManaAbilityParameters
                    {
                      Cost = new Sacrifice(),
                      Text = "Sacrifice this land: Add {B} to your mana pool.",
                      Priority = ManaSourcePriorities.Restricted,
                      TapRestriction = false
                    };

                  mp.ManaAmount(Mana.Black);

                  return new AddActivatedAbility(new ManaAbility(mp));
                });

            p.TimingRule(new Steps(activeTurn: true, passiveTurn: false, steps: Step.Upkeep));
          });
    }
  }
}
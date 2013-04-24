namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Core;
  using Gameplay.Card.Abilities;
  using Gameplay.Card.Factory;
  using Gameplay.Effects;
  using Gameplay.Modifiers;
  using Gameplay.States;

  public class Falter : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Falter")
        .ManaCost("{1}{R}")
        .Type("Instant")
        .Text("Creatures without flying can't block this turn.")
        .FlavorText("Like a sleeping dragon, Shiv stirs and groans at times.")
        .Cast(p =>
          {
            p.Effect = () => new ApplyModifiersToPlayer(
              selector: e => e.Controller.Opponent,
              modifiers: () =>
                {
                  var pr = new ContinuousEffectParameters
                    {
                      Modifier = () => new AddStaticAbility(Static.CannotBlock),
                      CardFilter = (card, effect) =>
                        (card.Is().Creature && !card.Has().Flying),
                    };

                  return new AddContiniousEffect(new ContinuousEffect(pr)) {UntilEot = true};
                });

            p.TimingRule(new Turn(active: true));
            p.TimingRule(new Steps(Step.BeginningOfCombat));
          });
    }
  }
}
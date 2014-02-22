namespace Grove.Library
{
  using System.Collections.Generic;
  using Gameplay;
  using Gameplay.AI.TimingRules;
  using Grove.Gameplay.Effects;
  using Grove.Gameplay.Modifiers;

  public class Falter : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
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
              selector: e => e.Controller,
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
            
            p.TimingRule(new OnYourTurn(Step.BeginningOfCombat));
          });
    }
  }
}
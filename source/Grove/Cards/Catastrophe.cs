namespace Grove.Cards
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Core.Ai;
  using Core.Decisions.Results;
  using Core.Dsl;
  using Core.Effects;

  public class Catastrophe : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Catastrophe")
        .ManaCost("{4}{W}{W}")
        .Type("Sorcery")
        .Text("Destroy all lands or all creatures. Creatures destroyed this way can't be regenerated.")
        .FlavorText(
          "Radiant's eyes flashed. 'Go, then,' the angel spat at Serra, 'and leave this world to those who truly care.'")
        .Cast(p =>
          {
            p.Timing = Timings.SecondMain();
            p.Category = EffectCategories.Destruction;
            p.Effect = Effect<Core.Effects.CustomizableEffect>(e =>
              {
                e.Choices(Choice(EffectChoiceOption.Lands, EffectChoiceOption.Creatures));

                e.Ai = p1 =>
                  {
                    var opponent = p1.Game.Players.GetOpponent(p1.Controller);

                    var opponentCreatureCount = opponent.Battlefield.Creatures.Count();
                    var yourCreatureCount = p1.Controller.Battlefield.Creatures.Count();

                    return opponentCreatureCount - yourCreatureCount > 0
                      ? new ChosenOptions(EffectChoiceOption.Creatures)
                      : new ChosenOptions(EffectChoiceOption.Lands);
                  };

                e.Text = "Destroy all #0.";

                e.ProcessResults = p1 =>
                  {
                    if (p1.Result.Options[0] == EffectChoiceOption.Lands)
                    {
                      p1.Game.Players.DestroyPermanents(card => card.Is().Land);
                      return;
                    }

                    p1.Game.Players.DestroyPermanents(
                      card => card.Is().Creature,
                      allowToRegenerate: false);
                  };
              });
          });

    }
  }
}
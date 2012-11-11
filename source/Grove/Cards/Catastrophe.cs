namespace Grove.Cards
{
  using System.Collections.Generic;
  using System.Linq;
  using Core;
  using Core.Ai;
  using Core.Controllers.Results;
  using Core.Details.Cards.Effects;
  using Core.Dsl;

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
        .Timing(Timings.SecondMain())
        .Category(EffectCategories.Destruction)
        .Effect<CustomizableEffect>(e =>
          {
            e.Choices(Choice(EffectChoiceOption.Lands, EffectChoiceOption.Creatures));

            e.Ai = p =>
              {
                var opponent = p.Game.Players.GetOpponent(p.Controller);

                var opponentCreatureCount = opponent.Battlefield.Creatures.Count();
                var yourCreatureCount = p.Controller.Battlefield.Creatures.Count();

                return opponentCreatureCount - yourCreatureCount > 0
                  ? new ChosenOptions(EffectChoiceOption.Creatures)
                  : new ChosenOptions(EffectChoiceOption.Lands);
              };

            e.Text = "Destroy all #0.";

            e.ProcessResults = p =>
              {
                if (p.Result.Options[0] == EffectChoiceOption.Lands)
                {
                  p.Game.Players.DestroyPermanents(card => card.Is().Land);
                  return;
                }

                p.Game.Players.DestroyPermanents(
                  card => card.Is().Creature,
                  allowToRegenerate: false);
              };
          });
    }
  }
}
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
      yield return C.Card
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

            e.ChooseAi = (self, game) =>
              {
                var opponent = self.Players.GetOpponent(self.Controller);

                var opponentCreatureCount = opponent.Battlefield.Creatures.Count();
                var yourCreatureCount = self.Controller.Battlefield.Creatures.Count();

                return opponentCreatureCount - yourCreatureCount > 0
                  ? new ChosenOptions(EffectChoiceOption.Creatures)
                  : new ChosenOptions(EffectChoiceOption.Lands);
              };

            e.Text = "Destroy all #0.";

            e.Logic = (self, chosen) =>
              {
                if (chosen.Options[0] == EffectChoiceOption.Lands)
                {
                  self.Players.DestroyPermanents(card => card.Is().Land);
                  return;
                }

                self.Players.DestroyPermanents(
                  card => card.Is().Creature,
                  allowToRegenerate: false);
              };
          });
    }
  }
}
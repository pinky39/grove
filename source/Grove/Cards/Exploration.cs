namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Modifiers;
  using Core.Dsl;

  public class Exploration : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Exploration")
        .ManaCost("{G}")
        .Type("Enchantment")
        .Text("You may play an additional land on each of your turns.")
        .FlavorText(
          "The first explorers found Argoth a storehouse of natural wealth—towering forests grown over rich veins of ore.")
        .Cast(p=> p.Timing = Timings.FirstMain())
        .Abilities(
          Continuous(c =>
            {
              c.PlayerFilter = (player, effect) => player == effect.Source.Controller;
              c.ModifierFactory = Modifier<IncreaseLandLimit>(m => m.Amount = 1);
            })
        );
    }
  }
}
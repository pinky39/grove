namespace Grove.Cards
{
  using System.Collections.Generic;
  using Ai.TimingRules;
  using Gameplay.Card.Factory;
  using Gameplay.Modifiers;

  public class Exploration : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Exploration")
        .ManaCost("{G}")
        .Type("Enchantment")
        .Text("You may play an additional land on each of your turns.")
        .FlavorText(
          "The first explorers found Argoth a storehouse of natural wealth—towering forests grown over rich veins of ore.")
        .Cast(p => p.TimingRule(new FirstMain()))
        .ContinuousEffect(p =>
          {
            p.Modifier = () => new IncreaseLandLimit();
            p.PlayerFilter = (player, effect) => player == effect.Source.Controller;
          });
    }
  }
}
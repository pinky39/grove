namespace Grove.Cards
{
  using System.Collections.Generic;
  using Artifical.CostRules;
  using Artifical.TimingRules;
  using Gameplay.Characteristics;
  using Gameplay.Effects;
  using Gameplay.Misc;
  using Gameplay.Modifiers;

  public class GoblinOffensive : CardTemplateSource
  {
    public override IEnumerable<CardTemplate> GetCards()
    {
      yield return Card
        .Named("Goblin Offensive")
        .ManaCost("{1}{R}{R}").HasXInCost()
        .Type("Sorcery")
        .Text("Put X 1/1 red Goblin creature tokens onto the battlefield.")
        .FlavorText("They certainly are.")
        .Cast(p =>
          {
            p.Effect = () => new CreateTokens(
              count: Value.PlusX,
              token: Card
                .Named("Goblin Token")
                .FlavorText(
                  "When you're a goblin, you don't have to step forward to be a hero—everyone else just has to step back.")
                .Power(1)
                .Toughness(1)
                .Type("Creature Token Goblin")
                .Colors(CardColor.Red)
              );

            p.TimingRule(new WhenYouHaveMana(6));
            p.CostRule(new XIsAvailableMana());
          });
    }
  }
}
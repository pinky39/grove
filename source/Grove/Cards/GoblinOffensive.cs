namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.CostRules;
  using Core.Ai.TimingRules;
  using Core.Dsl;
  using Core.Effects;
  using Core.Mana;
  using Core.Modifiers;

  public class GoblinOffensive : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
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
              tokens: Card
                .Named("Goblin Token")
                .FlavorText(
                  "'When you're a goblin, you don't have to step forward to be a hero—everyone else just has to step back'{EOL}—Biggum Flodrot, goblin veteran")
                .Power(1)
                .Toughness(1)
                .Type("Creature Token Goblin")
                .Colors(ManaColors.Red)
              );

            p.TimingRule(new ControllerHasMana(6));
            p.CostRule(new MaxAvailableMana(ManaUsage.Spells));
          });
    }
  }
}
namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards.Effects;
  using Core.Cards.Modifiers;
  using Core.Dsl;
  using Core.Mana;

  public class GoblinOffensive : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Goblin Offensive")
        .ManaCost("{1}{R}{R}")
        .Type("Sorcery")
        .Text("Put X 1/1 red Goblin creature tokens onto the battlefield.")
        .FlavorText("They certainly are.")
        .Cast(p =>
          {
            p.XCalculator = ChooseXAi.AtLeast(3);
            p.Effect = Effect<CreateTokens>(e =>
              {
                e.Count = Value.PlusX;
                e.Tokens(Card
                  .Named("Goblin Token")
                  .FlavorText(
                    "'When you're a goblin, you don't have to step forward to be a hero—everyone else just has to step back'{EOL}—Biggum Flodrot, goblin veteran")
                  .Power(1)
                  .Toughness(1)
                  .Type("Creature Token Goblin")
                  .Colors(ManaColors.Red));
              });
          });
    }
  }
}
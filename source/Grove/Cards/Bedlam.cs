namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Cards;
  using Core.Cards.Modifiers;
  using Core.Dsl;

  public class Bedlam : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return Card
        .Named("Bedlam")
        .ManaCost("{2}{R}{R}")
        .Type("Enchantment")
        .Text("Creatures can't block.")
        .FlavorText("Sometimes quantity, in the absence of quality, is good enough.")
        .Timing(Timings.FirstMain())
        .Abilities(
          Continuous(e =>
            {
              e.CardFilter = (card, source) => card.Is().Creature;
              e.ModifierFactory = Modifier<AddStaticAbility>(
                m => { m.StaticAbility = Static.CannotBlock; });
            }));
    }
  }
}
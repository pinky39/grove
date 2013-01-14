namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Dsl;
  using Core.Modifiers;

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
        .Cast(p => p.Timing = Timings.FirstMain())
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
namespace Grove.Cards
{
  using System;
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards;
  using Core.Details.Cards.Modifiers;
  using Core.Dsl;

  public class Bedlam : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Bedlam")
        .ManaCost("{2}{R}{R}")
        .Type("Enchantment")
        .Text("Creatures can't block.")
        .FlavorText("Sometimes quantity, in the absence of quality, is good enough.")
        .Timing(Timings.FirstMain())
        .Abilities(
          C.Continuous((e, c) =>
            {
              e.ModifierFactory = c.Modifier<AddStaticAbility>(
                (m, _) => { m.StaticAbility = Static.CannotBlock; });
            }));                
    }
  }
}
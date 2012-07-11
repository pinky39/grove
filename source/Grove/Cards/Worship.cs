namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Modifiers;
  using Core.Details.Cards.Preventions;
  using Core.Dsl;

  public class Worship : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Worship")
        .ManaCost("{3}{W}")
        .Type("Enchantment")
        .Text(
          "If you control a creature, damage that would reduce your life total to less than 1 reduces it to 1 instead.")
        .FlavorText("'Believe in the ideal, not the idol.'{EOL}—Serra")
        .Timing(Timings.FirstMain())
        .Abilities(
          C.Continuous((e, c) =>
            {
              e.ModifierFactory = c.Modifier<AddDamagePrevention>(
                (m, c0) => m.Prevention = c0.Prevention<PreventLifelossBelowOne>());
              e.CardFilter = delegate { return false; };
              e.PlayerFilter = (player, armor) => player == armor.Controller;
            })
        );
    }
  }
}
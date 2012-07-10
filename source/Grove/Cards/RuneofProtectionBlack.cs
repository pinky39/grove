namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Costs;
  using Core.Effects;

  public class RuneofProtectionBlack : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Rune of Protection: Black")
        .ManaCost("{1}{W}")
        .Type("Enchantment")
        .Text(
          "{W}: The next time a black source of your choice would deal damage to you this turn, prevent that damage.{EOL}Cycling {2} ({2}, Discard this card: Draw a card.)")
        .Timing(Timings.FirstMain())
        .Abilities(
          C.ActivatedAbility(
            "{W}: The next time a black source of your choice would deal damage to you this turn, prevent that damage.",
            C.Cost<TapOwnerPayMana>((cost, _) => { cost.Amount = ManaAmount.White; }),
            C.Effect<ApplyModifiersToSelf>((e, c) =>
              {
                
              })
          )                  
        );
    }
  }
}
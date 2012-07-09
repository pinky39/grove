namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.CardDsl;
  using Core.Effects;
  using Core.Modifiers;

  public class Pariah : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Pariah")
        .ManaCost("{3}{W}")
        .Type("Enchantment - Aura")
        .Timing(Timings.SecondMain())
        .Text("All damage that would be dealt to you is dealt to enchanted creature instead.")
        .FlavorText(
          "'It is not sad', Radiant chided the lesser angel. 'It is right. Every society must have its outcasts.'")
        .Effect<EnchantCreature>((e, c) =>
          {
            e.ModifiesEnchantmentController = true;
            e.Modifiers(
              c.Modifier<AddDamageRedirection>()
              );
          });
    }
  }
}
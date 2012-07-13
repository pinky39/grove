namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai;
  using Core.Details.Cards.Effects;
  using Core.Details.Cards.Modifiers;
  using Core.Details.Cards.Redirections;
  using Core.Dsl;
  using Core.Targeting;

  public class Pariah : CardsSource
  {
    public override IEnumerable<ICardFactory> GetCards()
    {
      yield return C.Card
        .Named("Pariah")
        .ManaCost("{2}{W}")
        .Type("Enchantment - Aura")
        .Timing(Timings.FirstMain())
        .Text("All damage that would be dealt to you is dealt to enchanted creature instead.")
        .FlavorText(
          "'It is not sad', Radiant chided the lesser angel. 'It is right. Every society must have its outcasts.'")
        .Effect<EnchantCreature>((e, c) =>
          {
            e.ModifiesEnchantmentController = true;
            e.Modifiers(
              c.Modifier<AddDamageRedirection>((m, c0) =>
                m.Redirection = c.Redirection<RedirectDamageToTarget>((r, c1) => { r.Target = m.Source.AttachedTo; }))
              );
          })
        .Targets(
          filter: TargetFilters.DamageRedirection(),
          effect: C.Selector(Selectors.EnchantedCreature()));
    }
  }
}